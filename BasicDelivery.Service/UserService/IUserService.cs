using BasicDelivery.Domain.Entities;
using BasicDelivery.Domain.ViewModel;
using BasicDelivery.ModelView;
using Microsoft.AspNetCore.Mvc;

namespace BasicDelivery.Service.UserService
{
    public interface IUserService
    {
        Task<User> FindByEmail(string email);
        Task<User> FindById(int? id);
        Task<IEnumerable<User>> GetUser();
        Task<ErrorMessage<User>> LoginUser(LoginViewModel model);

        //Task RegisterUser([FromForm] UserModelView model);
        Task<string> RegisterUser([FromForm] UserViewModel model);
    }
}