using BasicDelivery.Authentication.Service;
using BasicDelivery.Data;
using BasicDelivery.Data.Abstract;
using BasicDelivery.Domain.Entities;
using BasicDelivery.Service.DriverService;
using BasicDelivery.Service.DriverTokenService;
using BasicDelivery.Service.HistoryService;
using BasicDelivery.Service.OrderService;
using BasicDelivery.Service.UserService;
using BasicDelivery.Service.UserTokenService;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BasicDelivery.Infrastucture.Configuration
{
    public static class ConfiguarationService
    {
        public static void RegisterContextDb(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<deliveryDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("BasicDelivery"),
                                                                      options => options.MigrationsAssembly(typeof(deliveryDbContext).Assembly.FullName)));
        }

        public static void RegisterDI(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDriverService, DriverService>();
            services.AddScoped<IUserTokenService, UserTokenService>();
            services.AddScoped<ITokenHandler, TokenHandler>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IDriverTokenService, DriverTokenService>();
            services.AddScoped<IHistoryService, HistoryService>();
        }

        public static void RegisterController(this IServiceCollection services)
        {
            services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

        }

        public static void RegisterConnectAngular(this IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy(name: "WebBasicDelivery",
                policy =>
                {
                    policy.WithOrigins("http://localhost:4200").AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                }));
        }

        public static void RegisterSignalR(this IServiceCollection services)
        {
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });
        }
    }
}
