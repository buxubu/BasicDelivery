using AutoMapper;
using BasicDelivery.Authentication.Service;
using BasicDelivery.Data.Abstract;
using BasicDelivery.Data.Extentions;
using BasicDelivery.Domain.Entities;
using BasicDelivery.Domain.ViewModel;
using BasicDelivery.ModelView;
using BasicDelivery.Service.UserService;
using BasicDelivery.Service.UserTokenService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BasicDelivery.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ITokenHandler _tokenHandler;
        private readonly IUserTokenService _userTokenService;
        public UserController(IUserService userService, IMapper mapper, ITokenHandler tokenHandler, IUserTokenService userTokenService)
        {
            _userService = userService;
            _mapper = mapper;
            _tokenHandler = tokenHandler;
            _userTokenService = userTokenService;
        }
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                return Ok(await _userService.GetUser());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [HttpPost("register-user")]
        public async Task<IActionResult> Register([FromForm] UserViewModel model)
        {
            try
                {
                if (ModelState.IsValid)
                {
                    return Ok(await _userService.RegisterUser(model));
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                // Check if there is an inner exception
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");


            }
        }

        [HttpPost("login-user")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Not exist");
                }
                var checkLogin = await _userService.LoginUser(model);

                string ErrorMessage = string.Empty;
                bool checkPass = CheckPassword.ValidatePassword(model.PasswordHash, out ErrorMessage);
                if (checkPass == false)
                {
                    return BadRequest(ErrorMessage);
                }

                //return Ok(checkLogin);
                if (checkLogin.Entity != null)
                {
                    (string accessToken, DateTime expiredDateAccess) = await _tokenHandler.CreateAccessToken(checkLogin.Entity);
                    (string codeSerialNumber, string refreshToken, DateTime expiredRefreshAccess) = await _tokenHandler.CreateRefreshToken(/*checkLogin.Entity*/);


                    await _userTokenService.SaveToken(new UserToken
                    {
                        UserId = checkLogin.Entity.UserId,
                        AccessToken = accessToken,
                        ExpiredDateAccessToken = expiredDateAccess,
                        RefreshToken = refreshToken,
                        ExpiredRefreshToken = expiredRefreshAccess,
                        CodeRefreshToken = codeSerialNumber,
                        CreatedDate = DateTime.Now
                    });

                    return Ok(new JwtViewModel
                    {
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        Email = checkLogin.Entity.Email,
                        FullName = checkLogin.Entity.FullName
                    });
                }
                return BadRequest("Vui lòng kiểm tra lại tài khoản và mật khẩu.");


            }
            catch (Exception ex)
            {
                // Check if there is an inner exception
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }

        [HttpGet("refresh-token/{refreshToken}")]
        //[AllowAnonymous]
        public async Task<IActionResult> RefreshToken(/*[FromForm] RefreshTokenViewModel model*/ string refreshToken)
        {
            try
            {
                if (/*model*/RefreshToken == null) return BadRequest("Could not get model token!");

                return Ok(await _tokenHandler.ValidateRefreshToken(/*model.*/refreshToken));
            }
            catch (Exception ex)
            {
                // Check if there is an inner exception
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
            
        }

        [HttpGet("get-email/{email}")]
        public async Task<IActionResult> GetEmail(string? email)
        {
            try
            {
                if (email == null) return BadRequest("Email is null");
                var check = await _userService.FindByEmail(email);
                  if (check == null) return BadRequest("Not find user in database");

                return Ok(check);
            }
            catch (Exception ex)
            {
                // Check if there is an inner exception
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }

    }
}
