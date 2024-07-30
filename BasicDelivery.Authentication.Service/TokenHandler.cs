using BasicDelivery.Domain.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using BasicDelivery.Service.UserService;
using System.Threading.Tasks;
using BasicDelivery.Domain.ViewModel;
using BasicDelivery.Service.UserTokenService;
using BasicDelivery.Service.DriverService;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using BasicDelivery.Service.DriverTokenService;

namespace BasicDelivery.Authentication.Service
{
    public class TokenHandler : ITokenHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IDriverService _driverService;
        private readonly IUserTokenService _userTokenService;
        private readonly IDriverTokenService _driverTokenService;
        public TokenHandler(IConfiguration configuration, IUserService userService, IUserTokenService userTokenService, IDriverService driverService, IDriverTokenService driverTokenService)
        {
            _configuration = configuration;
            _userService = userService;
            _userTokenService = userTokenService;
            _driverService = driverService;
            _driverTokenService = driverTokenService;
        }
        public async Task<(string, DateTime)> CreateAccessToken(User user)
        {
            DateTime expiredToken = DateTime.UtcNow.ToLocalTime().AddMinutes(int.Parse(_configuration["TokenBear:AccessTokenExpiredByMinutes"]));
            var claims = new Claim[]
            {
                // key chinh
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString(),ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Iss,_configuration["TokenBear:Issuer"],ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
                // thoi gian tao token
                new Claim(JwtRegisteredClaimNames.Iat,DateTimeOffset.UtcNow.ToString(),ClaimValueTypes.DateTime, _configuration["TokenBear:Issuer"]),
                // tac gia
                new Claim(JwtRegisteredClaimNames.Aud,"BasicDelivery",ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
                // thoi gian het han token
                new Claim(JwtRegisteredClaimNames.Exp,expiredToken.ToString(),ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
                new Claim(ClaimTypes.Name,user.FullName,ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
                new Claim("Email",user.Email,ClaimValueTypes.String, _configuration["TokenBear:Issuer"])
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenBear:SignatureKey"]));
            // ma hoa key
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenInfo = new JwtSecurityToken(
                issuer: _configuration["TokenBear:Issuer"],
                audience: _configuration["TokenBear:Audience"],
                claims: claims,
                notBefore: DateTime.Now,
                expires: expiredToken,
                credential
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenInfo);

            return await Task.FromResult((token, expiredToken));
        }

        public async Task<(string, string, DateTime)> CreateRefreshToken(/*User user*/)
        {
            //luu het han token
            DateTime expiredRrefreshToken = DateTime.UtcNow.ToLocalTime().AddHours(int.Parse(_configuration["TokenBear:AccessTokenExpiredByHours"]));

            //lay guid duy nhat de thay the refresh token
            string codeSerialNumber = Guid.NewGuid().ToString();

            var claims = new Claim[]
            {
                // key chinh
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString(),ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Iss,_configuration["TokenBear:Issuer"],ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
                // thoi gian tao token
                new Claim(JwtRegisteredClaimNames.Iat,DateTimeOffset.UtcNow.ToString(),ClaimValueTypes.DateTime, _configuration["TokenBear:Issuer"]),new Claim(JwtRegisteredClaimNames.Aud,"BasicDelivery",ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
                // thoi gian het han token
                new Claim(JwtRegisteredClaimNames.Exp,expiredRrefreshToken.ToString(),ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
                new Claim(ClaimTypes.SerialNumber,codeSerialNumber,ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),

            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenBear:SignatureKey"]));
            // ma hoa key
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenInfo = new JwtSecurityToken(
                issuer: _configuration["TokenBear:Issuer"],
                audience: _configuration["TokenBear:Audience"],
                claims: claims,
                notBefore: DateTime.Now,
                expires: expiredRrefreshToken,
                credential
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenInfo);

            return await Task.FromResult((codeSerialNumber, token, expiredRrefreshToken));
        }

        public async Task<(string, DateTime)> CreateAccessTokenDriver(Driver driver)
        {
            DateTime expiredToken = DateTime.UtcNow.ToLocalTime().AddMinutes(int.Parse(_configuration["TokenBear:AccessTokenExpiredByMinutes"]));
            var claims = new Claim[]
            {
                // key chinh
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString(),ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Iss,_configuration["TokenBear:Issuer"],ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
                // thoi gian tao token
                new Claim(JwtRegisteredClaimNames.Iat,DateTimeOffset.UtcNow.ToString(),ClaimValueTypes.DateTime, _configuration["TokenBear:Issuer"]),
                // tac gia
                new Claim(JwtRegisteredClaimNames.Aud,"BasicDelivery",ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
                // thoi gian het han token
                new Claim(JwtRegisteredClaimNames.Exp,expiredToken.ToString(),ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
                new Claim(ClaimTypes.Name,driver.FullName,ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
                new Claim("Email",driver.Email,ClaimValueTypes.String, _configuration["TokenBear:Issuer"])
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenBear:SignatureKey"]));
            // ma hoa key
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenInfo = new JwtSecurityToken(
                issuer: _configuration["TokenBear:Issuer"],
                audience: _configuration["TokenBear:Audience"],
                claims: claims,
                notBefore: DateTime.Now,
                expires: expiredToken,
                credential
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenInfo);

            return await Task.FromResult((token, expiredToken));
        }

        //public async Task<(string, string, DateTime)> CreateRefreshTokenDriver()
        //{
        //    //luu het han token
        //    DateTime expiredRrefreshToken = DateTime.UtcNow.ToLocalTime().AddHours(int.Parse(_configuration["TokenBear:AccessTokenExpiredByHours"]));

        //    //lay guid duy nhat de thay the refresh token
        //    string codeSerialNumber = Guid.NewGuid().ToString();

        //    var claims = new Claim[]
        //    {
        //        // key chinh
        //        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString(),ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
        //        new Claim(JwtRegisteredClaimNames.Iss,_configuration["TokenBear:Issuer"],ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
        //        // thoi gian tao token
        //        new Claim(JwtRegisteredClaimNames.Iat,DateTimeOffset.UtcNow.ToString(),ClaimValueTypes.DateTime, _configuration["TokenBear:Issuer"]),new Claim(JwtRegisteredClaimNames.Aud,"BasicDelivery",ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
        //        // thoi gian het han token
        //        new Claim(JwtRegisteredClaimNames.Exp,expiredRrefreshToken.ToString(),ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),
        //        new Claim(ClaimTypes.SerialNumber,codeSerialNumber,ClaimValueTypes.String, _configuration["TokenBear:Issuer"]),

        //    };

        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenBear:SignatureKey"]));
        //    // ma hoa key
        //    var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        //    var tokenInfo = new JwtSecurityToken(
        //        issuer: _configuration["TokenBear:Issuer"],
        //        audience: _configuration["TokenBear:Audience"],
        //        claims: claims,
        //        notBefore: DateTime.Now,
        //        expires: expiredRrefreshToken,
        //        credential
        //        );

        //    string token = new JwtSecurityTokenHandler().WriteToken(tokenInfo);

        //    return await Task.FromResult((codeSerialNumber, token, expiredRrefreshToken));
        //}

        public async Task ValidateToken(TokenValidatedContext context)
        {
            var claims = context.Principal.Claims.ToList();
            if (claims.Count == 0)
            {
                context.Fail("This token contains no information");
                return;
            }

            var identity = context.Principal.Identity as ClaimsIdentity;
            if (identity.FindFirst(JwtRegisteredClaimNames.Iss) == null)
            {
                context.Fail("This token is not issued by point entry");
                return;
            }

            if (identity.FindFirst("Email") != null)
            {
                string email = identity.FindFirst("Email").Value;
                User checkEmailUser = await _userService.FindByEmail(email);
                Driver checkEmailDriver = await _driverService.FindByEmail(email);
                if (checkEmailUser == null && checkEmailDriver == null)
                {
                    context.Fail("This token is invalid for user");
                    return;
                }

            }

            if (identity.FindFirst(JwtRegisteredClaimNames.Exp) != null)
            {
                var dateExp = identity.FindFirst(JwtRegisteredClaimNames.Exp).Value;
                long ticks = long.Parse(dateExp);
                // ngay het han
                var date = DateTimeOffset.FromUnixTimeSeconds(ticks).UtcDateTime;
                //nagy bay gio dc convert
                var now = DateTime.Now.ToUniversalTime();

                if (date <= now)
                {
                    context.Fail("This token is expired");
                    throw new Exception("This token is expired");
                }


            }

        }

        public async Task<JwtViewModel> ValidateRefreshToken(string refreshToken)
        {
            JwtViewModel jwtModel = new JwtViewModel();
            var cliamPriciple = new JwtSecurityTokenHandler().ValidateToken(
                refreshToken,
                new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenBear:SignatureKey"])),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                },
                out _
                );

            if (cliamPriciple == null) return jwtModel;

            string serialNumber = cliamPriciple.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber)?.Value;

            UserToken checkSerialNumber = await _userTokenService.FindSerialNumber(serialNumber);

            if (checkSerialNumber != null)
            {
                User user = await _userService.FindById(checkSerialNumber.UserId);

                (string newAccessToken, DateTime newExpiredAccessToken) = await CreateAccessToken(user);
                (string codeSerialNumber, string newRefreshAccessToken, DateTime newExpiredRrefreshToken) = await CreateRefreshToken(/*user*/);

                await _userTokenService.SaveToken(new UserToken
                {
                    UserId = user.UserId,
                    AccessToken = newAccessToken,
                    ExpiredDateAccessToken = newExpiredAccessToken,
                    RefreshToken = newRefreshAccessToken,
                    ExpiredRefreshToken = newExpiredRrefreshToken,
                    CodeRefreshToken = codeSerialNumber,
                    CreatedDate = DateTime.Now
                });

                return new JwtViewModel
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshAccessToken,
                    FullName = user.FullName,
                    Email = user.Email
                };
            }
            return new();
        }


        public async Task<JwtViewModel> ValidateRefreshTokenDriver(string refreshToken)
        {
            JwtViewModel jwtModel = new JwtViewModel();
            var cliamPriciple = new JwtSecurityTokenHandler().ValidateToken(
                refreshToken,
                new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenBear:SignatureKey"])),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                },
                out _
                );

            if (cliamPriciple == null) return jwtModel;

            string serialNumber = cliamPriciple.Claims.FirstOrDefault(x => x.Type == ClaimTypes.SerialNumber)?.Value;

            DriverToken checkSerialNumber = await _driverTokenService.FindSerialNumber(serialNumber);
            if (checkSerialNumber != null)
            {
                Driver driver = await _driverService.FindById(checkSerialNumber.DriverId);

                (string newAccessToken, DateTime newExpiredAccessToken) = await CreateAccessTokenDriver(driver);
                (string codeSerialNumber, string newRefreshAccessToken, DateTime newExpiredRrefreshToken) = await CreateRefreshToken(/*user*/);

                await _driverTokenService.SaveToken(new DriverToken
                {
                    DriverId = driver.DriverId,
                    AccessToken = newAccessToken,
                    ExpiredDateAccessToken = newExpiredAccessToken,
                    RefreshToken = newRefreshAccessToken,
                    ExpiredRefreshToken = newExpiredRrefreshToken,
                    CodeRefreshToken = codeSerialNumber,
                    CreatedDate = DateTime.Now
                });

                return new JwtViewModel
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshAccessToken,
                    FullName = driver.FullName,
                    Email = driver.Email
                };
            }
            return new();
        }
    }
}
