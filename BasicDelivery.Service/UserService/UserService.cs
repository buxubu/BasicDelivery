using AutoMapper;
using BasicDelivery.Data;
using BasicDelivery.Data.Abstract;
using BasicDelivery.Data.Extentions;
using BasicDelivery.Domain.Entities;
using BasicDelivery.Domain.ViewModel;
using BasicDelivery.Extentions;
using BasicDelivery.ModelView;
using BasicDelivery.Service.UserService;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BasicDelivery.Service.UserService
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly IMapper _mapper;
        public UserService(IRepository<User> userRepository, IWebHostEnvironment webHostEnviroment, IMapper mapper)
        {
            _userRepository = userRepository;
            _webHostEnviroment = webHostEnviroment;
            _mapper = mapper;
        }

        public async Task<IEnumerable<User>> GetUser()
        {
            return await _userRepository.GetListT();
        }

        public async Task<string> RegisterUser([FromForm] UserViewModel model)
        {
            if (model.UploadAvatar != null)
            {
                string directoryPathImages = Path.Combine(_webHostEnviroment.ContentRootPath, "UploadFile");

                string filePathImages = Path.Combine(directoryPathImages, model.UploadAvatar.FileName);
                // check duoi
                var supportedFileTypes = new[] { "jpg.", "jpeg", "png", "gif" };
                var fileExt = System.IO.Path.GetExtension(model.UploadAvatar.FileName).Substring(1);
                if (supportedFileTypes.Contains(fileExt.ToLower()))
                {
                    await model.UploadAvatar.CopyToAsync(new FileStream(filePathImages, FileMode.Create));
                }
                string hostUrl = "https://localhost:7130/";
                model.Avatar = hostUrl + "UploadFile/" + model.UploadAvatar.FileName;
            }

            //check pass
            string ErrorMessage = string.Empty;
            bool checkPass = CheckPassword.ValidatePassword(model.PasswordHash, out ErrorMessage);
            if (checkPass == true)
            {
                model.Salt = HashMD5.GetSailt();
                model.PasswordHash = HashMD5.ToMD5(model.PasswordHash) + model.Salt;
                model.CreateDate = DateTime.UtcNow.ToLocalTime();
                model.LastLogin = null;
                model.Active = true;
                model.Role = "User";
                var mapToUser = _mapper.Map<User>(model);
                await _userRepository.Insert(mapToUser);
                await _userRepository.Commit();
                return "Success";
            }
            else
            {
                return ErrorMessage.ToString();
            }
        }

        public async Task<ErrorMessage<User>> LoginUser(LoginViewModel model)
        {
            var checkEmail = await _userRepository.GetSingleByConditionAsync(x => x.Email.ToLower().Trim() == model.Email.ToLower().Trim() && x.Role == "User");
            
            if (checkEmail == null)
            {
                return new ErrorMessage<User>
                {
                    Entity = null,
                    Code = 500,
                    Message = "Email not loud!"
                };
            }

            bool md5Pass = (HashMD5.ToMD5(model.PasswordHash) + checkEmail.Salt) == checkEmail.PasswordHash;
            if (md5Pass == false)
            {
                return new ErrorMessage<User>
                {
                    Entity = null,
                    Code = 500,
                    Message = "Password not loud!"
                };
            }



            checkEmail.LastLogin = DateTime.Now;
            _userRepository.Update(checkEmail);
            await _userRepository.Commit();

            return new ErrorMessage<User>
            {
                Entity = checkEmail,
                Code = 200,
                Message = "Login Success"
            };
        }

        public async Task<User> FindByEmail(string email)
        {
            return await _userRepository.GetSingleByConditionAsync(x => x.Email == email);
        }

        public async Task<User> FindById(int? id)
        {
            return await _userRepository.GetById(id);
        }

    }
}
