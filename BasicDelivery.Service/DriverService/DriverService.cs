using AutoMapper;
using BasicDelivery.Data.Abstract;
using BasicDelivery.Data.Extentions;
using BasicDelivery.Domain.Entities;
using BasicDelivery.Domain.ViewModel;
using BasicDelivery.Extentions;
using BasicDelivery.Helper;
using BasicDelivery.ModelView;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicDelivery.Service.DriverService
{
    public class DriverService : IDriverService
    {
        private readonly IRepository<Driver> _driverRepository;
        private readonly IRepository<DriverDetail> _driverDetailRepository;
        private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly IMapper _mapper;
        public DriverService(IRepository<Driver> driverRepository, IWebHostEnvironment webHostEnviroment, IMapper mapper, IRepository<DriverDetail> driverDetailRepository)
        {
            _driverRepository = driverRepository;
            _webHostEnviroment = webHostEnviroment;
            _mapper = mapper;
            _driverDetailRepository = driverDetailRepository;
        }

        public async Task<IEnumerable<Driver>> GetDriver()
        {
            return await _driverRepository.GetListT();
        }

        public async Task<string> RegisterDriver([FromForm] DriverViewModel model)
        {
            List<IFormFile> files = new List<IFormFile>
            {
                model.UploadAvatar,
                model.UploadFont,
                model.UploadBack
            };

            //up file
            //List<string> nameFile = await Unilities.UploadFile(files);
            //string directoryPathImages = Path.Combine(_webHostEnviroment.ContentRootPath, "UploadFile");
            //foreach (var item in files)
            //{
            //    string filePathImages = Path.Combine(directoryPathImages, item.FileName);
            //    //check chuoi phai la hinh anh khong
            //    var supportedFileTypes = new[] { "jpg", "jpeg", "png", "gif" };
            //    var fileI = System.IO.Path.GetExtension(item.FileName).Substring(1);

            //    if (supportedFileTypes.Contains(fileI.ToLower()))
            //    {
            //        await model.UploadAvatar.CopyToAsync(new FileStream(filePathImages, FileMode.Create));
            //    }
            //    else
            //    {
            //        return item.FileName + " ko dung dinh dang";
            //    }
            //}


            // upload mutiple imaes
            await Unilities.UploadMutipleImages(files);

            string hostUrl = "https://localhost:7130/";
            model.Avatar = hostUrl + "UploadFile/" + model.UploadAvatar.FileName;

            //check pass
            string ErrorMessage = string.Empty;
            bool checkPass = CheckPassword.ValidatePassword(model.PasswordHash, out ErrorMessage);
            if (checkPass == true)
            {
                model.Salt = HashMD5.GetSailt();
                model.PasswordHash = HashMD5.ToMD5(model.PasswordHash) + model.Salt;
                model.CreateDate = DateTime.Now;
                model.LastLogin = null;
                model.Active = true;
                model.Role = "Driver";
                model.ReviewRate = 0;
                var mapToDriver = _mapper.Map<Driver>(model);
                await _driverRepository.Insert(mapToDriver);
                await _driverRepository.Commit();

                //driverDetail
                model.Font = hostUrl + "UploadFile/" + model.UploadFont.FileName;
                model.Back = hostUrl + "UploadFile/" + model.UploadBack.FileName;
                model.DriverId = mapToDriver.DriverId;
                var mapToDiverDetail = _mapper.Map<DriverDetail>(model);
                await _driverDetailRepository.Insert(mapToDiverDetail);
                await _driverRepository.Commit();

                return "Success";
            }
            else
            {
                return ErrorMessage.ToString();
            }
        }

        public async Task<ErrorMessage<Driver>> LoginDriver(LoginViewModel model)
        {
            var checkEmail = await _driverRepository.GetSingleByConditionAsync(x => x.Email.ToLower().Trim() == model.Email.ToLower().Trim() && x.Role == "Driver");
            if (checkEmail == null)
            {
                return new ErrorMessage<Driver>
                {
                    Entity = null,
                    Code = 500,
                    Message = "Email not loud!"
                };
            }
            bool md5Pass = (HashMD5.ToMD5(model.PasswordHash) + checkEmail.Salt) == checkEmail.PasswordHash;
            if (md5Pass == false)
            {
                return new ErrorMessage<Driver>
                {
                    Entity = null,
                    Code = 500,
                    Message = "Password not loud!"
                };
            }
            checkEmail.LastLogin = DateTime.Now;
            _driverRepository.Update(checkEmail);
            await _driverRepository.Commit();

            return new ErrorMessage<Driver>
            {
                Entity = checkEmail,
                Code = 200,
                Message = "Login Success"
            };
        }

        public async Task<Driver> FindByEmail(string email)
        {
            return await _driverRepository.GetSingleByConditionAsync(x => x.Email == email);
        }

        public async Task<Driver> FindById(int? id)
        {
            return await _driverRepository.GetById(id);
        }
    }
}
