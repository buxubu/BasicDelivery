using AutoMapper;
using BasicDelivery.Domain.Entities;
using BasicDelivery.Domain.ViewModel;
using BasicDelivery.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicDelivery.Domain.Helper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<UserViewModel, User>().ReverseMap();
            CreateMap<DriverViewModel, Driver>().ReverseMap();
            CreateMap<DriverDetailViewModel, DriverDetail>().ReverseMap();
            CreateMap<OrderViewModel, Order>().ReverseMap();
            CreateMap<OrderDetailViewModel, OrderDetail>().ReverseMap();

        }
    }
}
