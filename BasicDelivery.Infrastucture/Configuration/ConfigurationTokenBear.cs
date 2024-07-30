
using BasicDelivery.Authentication.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicDelivery.Infrastucture.Configuration
{
    public static class ConfigurationTokenBear
    {
        public static void RegisterTokenBear(this IServiceCollection servers, IConfiguration configuration)
        {
            servers.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                   .AddJwtBearer(options =>
                   {
                       options.SaveToken = true;
                       options.TokenValidationParameters = new TokenValidationParameters
                       {
                           ValidIssuer = configuration["TokenBear:Issuer"],
                           // ma hoa issuer
                           ValidateIssuer = false,
                           ValidAudience = configuration["TokenBear:Audience"],
                           // ma hoa tac gia
                           ValidateAudience = false,
                           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenBear:SignatureKey"])),
                           ValidateIssuerSigningKey = true,
                           ValidateLifetime = true,
                           ClockSkew = TimeSpan.Zero
                       };
                       options.Events = new JwtBearerEvents()
                       {
                           OnTokenValidated = context =>
                           {
                               var tokenHandler = context.HttpContext.RequestServices.GetRequiredService<ITokenHandler>();
                               return tokenHandler.ValidateToken(context);
                           },
                           OnAuthenticationFailed = context =>
                           {
                               return Task.CompletedTask;
                           },
                           OnMessageReceived = context =>
                           {
                               return Task.CompletedTask;
                           },
                           OnChallenge = context =>
                           {
                               return Task.CompletedTask;
                           }
                       };
                   });
        }   
    }
}
