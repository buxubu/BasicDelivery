using BasicDelivery.Domain.Entities;

namespace BasicDelivery.Service.DriverTokenService
{
    public interface IDriverTokenService
    {
        Task<DriverToken> FindSerialNumber(string serialNmber);
        Task SaveToken(DriverToken model);
    }
}