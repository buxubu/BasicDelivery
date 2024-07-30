using AutoMapper;
using BasicDelivery.Authentication.Service;
using BasicDelivery.ModelView;
using BasicDelivery.Service.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BasicDelivery.Service.DriverService;
using BasicDelivery.Domain.ViewModel;
using BasicDelivery.Domain.Entities;
using BasicDelivery.Service.DriverTokenService;

namespace BasicDelivery.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DriverController : Controller
    {
        private readonly IDriverService _driverService;
        private readonly IMapper _mapper;
        private readonly ITokenHandler _tokenHandler;
        private readonly IDriverTokenService _driverTokenService;
        public DriverController(IDriverService driverService, IMapper mapper, ITokenHandler tokenHandler,
                                IDriverTokenService driverTokenService)
        {
            _driverService = driverService;
            _mapper = mapper;
            _tokenHandler = tokenHandler;
            _driverTokenService = driverTokenService;
        }
        //[HttpGet]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetUser()
        //{
        //    try
        //    {
        //        return Ok(await _driverService.GetDriver());
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message.ToString());
        //    }
        //}

        [HttpGet("{id:int}/getDriverId")]
        public async Task<IActionResult> GetDriverWthId(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return Ok(await _driverService.FindById(id));
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

        [HttpPost("register-driver")]
        public async Task<IActionResult> Register([FromForm] DriverViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return Ok(await _driverService.RegisterDriver(model));
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

        [HttpPost("login-driver")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Not exist");
                }
                var checkLogin = await _driverService.LoginDriver(model);
                if (checkLogin == null)
                {
                    return Unauthorized();
                }

                //return Ok(checkLogin);
                if (checkLogin.Entity != null)
                {
                    (string accessToken, DateTime expiredDateAccess) = await _tokenHandler.CreateAccessTokenDriver(checkLogin.Entity);
                    (string codeSerialNumber, string refreshToken, DateTime expiredRefreshAccess) = await _tokenHandler.CreateRefreshToken(/*checkLogin.Entity*/);

                    await _driverTokenService.SaveToken(new DriverToken
                    {
                        DriverId = checkLogin.Entity.DriverId,
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
                return BadRequest();


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

                return Ok(await _tokenHandler.ValidateRefreshTokenDriver(/*model.*/refreshToken));
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
                var check = await _driverService.FindByEmail(email);
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
