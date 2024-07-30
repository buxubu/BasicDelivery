using AutoMapper;
using BasicDelivery.Data.Abstract;
using BasicDelivery.Data.Migrations;
using BasicDelivery.Domain.Entities;
using BasicDelivery.Domain.ViewModel;
using BasicDelivery.Service.DriverService;
using BasicDelivery.Service.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicDelivery.Service.HistoryService
{
    public class HistoryService : IHistoryService
    {
        private readonly IRepository<History> _historyService;
        private readonly IRepository<Order> _orderService;
        private readonly IRepository<Driver> _driverReponsitory;
        private readonly IUserService _userService;
        private readonly IDriverService _driverService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly deliveryDbContext _db;
        public HistoryService(IRepository<History> historyService, IMapper mapper, IRepository<Order> orderService,
                              IRepository<Driver> driverReponsitory, deliveryDbContext db,
                              IHttpContextAccessor contextAccessor, IUserService userService,
                              IDriverService driverService)
        {
            _historyService = historyService;
            _orderService = orderService;
            _driverReponsitory = driverReponsitory;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _db = db;
            _userService = userService;
            _driverService = driverService;
        }

        public Task<History> EditHistory(History model, int idHistory)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<HistoryViewModel>> GetHistories()
        {
            var email = _contextAccessor.HttpContext.User.FindFirst("Email")?.Value;

            User user = await _userService.FindByEmail(email);

            var getAllHistory = await _historyService.GetListT(x=>x.Order.UserId == user.UserId);
            
            List<HistoryViewModel> listHisViewModel = new List<HistoryViewModel>();

            foreach (var item in getAllHistory)
            {
                HistoryViewModel hisViewModel = new HistoryViewModel();

                var getIdOrder = await _orderService.GetById(item.OrderId);

                var getIdDriver = await _driverReponsitory.GetById(item.DriverId);

                hisViewModel.HistoryId = item.HistoryId;
                hisViewModel.OrderId = item.OrderId;
                hisViewModel.ReceiverAddress = getIdOrder.ReceiverAddress;
                hisViewModel.ReceiverPhone = getIdOrder.ReceiverPhone;
                hisViewModel.ReceiverName = getIdOrder.ReceiverName;
                hisViewModel.TotalMoneyOrder = getIdOrder.TotalCod;
                if (getIdOrder.PaymentMethod == true)
                {
                    hisViewModel.PaymentString = "Người nhận trả tiền";
                }
                else
                {
                    hisViewModel.PaymentString = "Người giao trả tiền";
                }
                hisViewModel.Status = getIdOrder.Status;
                IEnumerable<StatusOrder> lstStatus = await this.GetListStatus();

                foreach (var itemStatus in lstStatus)
                {
                    if (getIdOrder.Status == itemStatus.StatusInt)
                    {
                        hisViewModel.StatusDetail = itemStatus.Status;
                        break;
                    }
                };
                hisViewModel.OrderDate = item.OrderDate;
                hisViewModel.ChangeDate = item.ChangeDate;
                if (item.DriverId != null)
                {
                    hisViewModel.DriverId = item.DriverId;
                    hisViewModel.DriverName = getIdDriver.FullName;
                }
                else
                {
                    hisViewModel.DriverId = null;
                    hisViewModel.DriverName = "Chưa có tài xế nhận đơn";
                }
                
                hisViewModel.Reason = item.Reason;
                listHisViewModel.Add(hisViewModel);
            }
            return listHisViewModel;
        }

        public async Task<IEnumerable<HistoryViewModel>> GetHistoriesDriver()
        {
            var email = _contextAccessor.HttpContext.User.FindFirst("Email")?.Value;

            Driver driver = await _driverService.FindByEmail(email);

            var getAllHistory = await _historyService.GetListT(x => x.Order.DriverId == driver.DriverId);

            List<HistoryViewModel> listHisViewModel = new List<HistoryViewModel>();

            foreach (var item in getAllHistory)
            {
                HistoryViewModel hisViewModel = new HistoryViewModel();

                var getIdOrder = await _orderService.GetById(item.OrderId);

                var getIdDriver = await _driverReponsitory.GetById(item.DriverId);

                hisViewModel.HistoryId = item.HistoryId;
                hisViewModel.OrderId = item.OrderId;
                hisViewModel.ReceiverAddress = getIdOrder.ReceiverAddress;
                hisViewModel.ReceiverPhone = getIdOrder.ReceiverPhone;
                hisViewModel.ReceiverName = getIdOrder.ReceiverName;
                hisViewModel.TotalMoneyOrder = getIdOrder.TotalCod;
                if (getIdOrder.PaymentMethod == true)
                {
                    hisViewModel.PaymentString = "Người nhận trả tiền";
                }
                else
                {
                    hisViewModel.PaymentString = "Người giao trả tiền";
                }
                hisViewModel.Status = getIdOrder.Status;
                IEnumerable<StatusOrder> lstStatus = await this.GetListStatus();

                foreach (var itemStatus in lstStatus)
                {
                    if (getIdOrder.Status == itemStatus.StatusInt)
                    {
                        hisViewModel.StatusDetail = itemStatus.Status;
                        break;
                    }
                };
                hisViewModel.OrderDate = item.OrderDate;
                hisViewModel.ChangeDate = item.ChangeDate;
                if (item.DriverId != null)
                {
                    hisViewModel.DriverId = item.DriverId;
                    hisViewModel.DriverName = getIdDriver.FullName;
                }
                else
                {
                    hisViewModel.DriverId = null;
                    hisViewModel.DriverName = "Chưa có tài xế nhận đơn";
                }

                hisViewModel.Reason = item.Reason;
                listHisViewModel.Add(hisViewModel);
            }
            return listHisViewModel;
        }
        public async Task<IEnumerable<StatusOrder>> GetListStatus()
        {
            return await _db.StatusOrders.ToListAsync();
        }

        public async Task InsertHistory(History model)
        {
            await _historyService.Insert(model);
            await _historyService.Commit();
        }
    }
}
