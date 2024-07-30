using BasicDelivery.Domain.Entities;

namespace BasicDelivery.Service.UserTokenService
{
    public interface IUserTokenService
    {
        Task<UserToken> FindSerialNumber(string serialNmber);
        Task SaveToken(UserToken model);
    }
}