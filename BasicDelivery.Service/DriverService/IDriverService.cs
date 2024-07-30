using BasicDelivery.Domain.Entities;
using BasicDelivery.Domain.ViewModel;
using BasicDelivery.ModelView;
using Microsoft.AspNetCore.Mvc;

namespace BasicDelivery.Service.DriverService
{
    public interface IDriverService
    {
        Task<Driver> FindByEmail(string email);
        Task<Driver> FindById(int? id);
        Task<IEnumerable<Driver>> GetDriver();
        Task<ErrorMessage<Driver>> LoginDriver(LoginViewModel model);
        Task<string> RegisterDriver([FromForm] DriverViewModel model);
    }
}