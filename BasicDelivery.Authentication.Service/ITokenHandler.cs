using BasicDelivery.Domain.Entities;
using BasicDelivery.Domain.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BasicDelivery.Authentication.Service
{
    public interface ITokenHandler
    {
        Task<(string, DateTime)> CreateAccessToken(User user);
        Task<(string, DateTime)> CreateAccessTokenDriver(Driver driver);
        Task<(string, string, DateTime)> CreateRefreshToken(/*User user*/);
        //Task<(string, string, DateTime)> CreateRefreshTokenDriver(Driver driver);
        Task<JwtViewModel> ValidateRefreshToken(string refreshToken);
        Task<JwtViewModel> ValidateRefreshTokenDriver(string refreshToken);
        Task ValidateToken(TokenValidatedContext context);
    }
}