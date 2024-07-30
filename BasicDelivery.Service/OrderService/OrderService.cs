using AutoMapper;
using BasicDelivery.Data.Abstract;
using BasicDelivery.Domain.Entities;
using BasicDelivery.Domain.ViewModel;
using BasicDelivery.Helper;
using BasicDelivery.Service.DriverService;
using BasicDelivery.Service.UserService;
using ExcelDataReader;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Excel = Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using BasicDelivery.Service.HistoryService;

namespace BasicDelivery.Service.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly deliveryDbContext _db;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Driver> _driverRepository;
        private readonly IRepository<OrderDetail> _orderDetailRepository;
        private readonly IUserService _userService;
        private readonly IDriverService _driverService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IHistoryService _historyService;
        //private readonly IWebHostEnvironment _webHostEnviroment;
        private readonly IMapper _mapper;
        public OrderService(IRepository<Order> orderRepository, /*IWebHostEnvironment webHostEnviroment,*/
                            IRepository<OrderDetail> orderDetailRepository, IUserService userService,
                            IRepository<User> userRepository, IRepository<Driver> driverRepository,
                            IMapper mapper, IHttpContextAccessor contextAccessor, IDriverService driverService,
                            IHistoryService historyService,
                            deliveryDbContext db)
        {
            _orderRepository = orderRepository;
            //_webHostEnviroment = webHostEnviroment;
            _orderDetailRepository = orderDetailRepository;
            _userService = userService;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _driverService = driverService;
            _userRepository = userRepository;
            _driverRepository = driverRepository;
            _historyService = historyService;
            _db = db;
        }

        // lấy ra bảng order đã dc nhận
        public async Task<IEnumerable<Order>> GetListOrderUser()
        {
            var email = _contextAccessor.HttpContext.User.FindFirst("Email")?.Value;

            User user = await _userService.FindByEmail(email);
            return await _orderRepository.GetListTInclu(x => x.UserId == user.UserId, x => x.Active == true, x => x.OrderDetails);
        }

        public async Task<IEnumerable<Order>> GetListOrderUserCreated()
        {
            var email = _contextAccessor.HttpContext.User.FindFirst("Email")?.Value;

            User user = await _userService.FindByEmail(email);
            return await _orderRepository.GetListTInclu(x => x.UserId == user.UserId, x => x.Active == true && x.DriverId == null && x.Status == 0, x => x.OrderDetails);
        }

        public async Task<IEnumerable<Order>> GetListOrderUserDeliver()
        {
            var email = _contextAccessor.HttpContext.User.FindFirst("Email")?.Value;

            User user = await _userService.FindByEmail(email);
            return await _orderRepository.GetListTInclu(x => x.UserId == user.UserId, x => x.Active == true && x.DriverId != null && x.Status == 1, x => x.OrderDetails);
        }

        public async Task<IEnumerable<Order>> GetListOrderUserCompleted()
        {
            var email = _contextAccessor.HttpContext.User.FindFirst("Email")?.Value;

            User user = await _userService.FindByEmail(email);
            return await _orderRepository.GetListTInclu(x => x.UserId == user.UserId, x => x.Status == 5, x => x.OrderDetails);
        }

        public async Task<IEnumerable<Order>> GetListOrderUserCancel()
        {
            var email = _contextAccessor.HttpContext.User.FindFirst("Email")?.Value;

            User user = await _userService.FindByEmail(email);
            return await _orderRepository.GetListTInclu(x => x.UserId == user.UserId, x => x.Status == 3, x => x.OrderDetails);
        }

        public async Task<IEnumerable<Order>> GetListOrderUserLost()
        {
            var email = _contextAccessor.HttpContext.User.FindFirst("Email")?.Value;

            User user = await _userService.FindByEmail(email);
            return await _orderRepository.GetListTInclu(x => x.UserId == user.UserId, x => x.Status == 4, x => x.OrderDetails);
        }

        public async Task<IEnumerable<Order>> GetListOrderUserFail()
        {
            var email = _contextAccessor.HttpContext.User.FindFirst("Email")?.Value;

            User user = await _userService.FindByEmail(email);
            return await _orderRepository.GetListTInclu(x => x.UserId == user.UserId, x => x.Status == 2, x => x.OrderDetails);
        }

        // driver
        public async Task<IEnumerable<Order>> GetListOrderDriverCompleted()
        {
            var email = _contextAccessor.HttpContext.User.FindFirst("Email")?.Value;

            Driver driver = await _driverService.FindByEmail(email);
            return await _orderRepository.GetListTInclu(x => x.DriverId == driver.DriverId, x => x.Status == 5, x => x.OrderDetails);
        }

        public async Task<IEnumerable<Order>> GetAllOrderDriver()
        {
            var email = _contextAccessor.HttpContext.User.FindFirst("Email")?.Value;

            Driver driver = await _driverService.FindByEmail(email);
            return await _orderRepository.GetListTInclu(x => x.DriverId == driver.DriverId, x => x.Active == true, x => x.OrderDetails);
        }

        public async Task<IEnumerable<Order>> GetListOrderDriver()
        {
            return await _orderRepository.GetListTInclu(null, x => x.Status == 0 && x.Active == true && x.DriverId == null, x => x.OrderDetails);
        }

        public async Task<IEnumerable<Order>> GetListOrderDriverAccepted()
        {
            var email = _contextAccessor.HttpContext.User.FindFirst("Email")?.Value;

            Driver driver = await _driverService.FindByEmail(email);
            return await _orderRepository.GetListTInclu(x => x.DriverId == driver.DriverId, x => x.Status == 1 && x.Active == true && x.DriverId == driver.DriverId, x => x.OrderDetails);
        }

        public async Task<Order> GetDetailOrderById(int id = 0)
        {
            return await _orderRepository.GetDetailByIdInclu(x => x.OrderId == id, x => x.OrderDetails);
        }

        public async Task<ErrorMessage<OrderViewModel>> InsertOrder([FromForm] OrderViewModel model)
        {
            if (_contextAccessor.HttpContext is null)
            {
                return new ErrorMessage<OrderViewModel>
                {
                    Code = 500,
                    Message = "HttpContext is null"
                };
            }
            var email = _contextAccessor.HttpContext.User.FindFirst("Email")?.Value;
            if (email == null)
            {
                return new ErrorMessage<OrderViewModel>
                {
                    Code = 500,
                    Message = "Can't get email claim"
                };
            }

            User user = await _userService.FindByEmail(email);

            List<IFormFile> files = new List<IFormFile>();

            if (model.ListOrderDetail != null)
            {
                foreach (var item in model.ListOrderDetail)
                {
                    if (item.UploadImagesProduct != null)
                    {
                        files.Add(item.UploadImagesProduct);
                    }
                }
            }


            if (model.UploadImagesPackage != null)
            {
                files.Add(model.UploadImagesPackage);
            }


            await Unilities.UploadMutipleImages(files);

            string hostUrl = "https://localhost:7130/";



            if (model.TotalGamPackage < 1000)
            {
                model.ShipCost = 30000;
            }
            model.OrderId = 0;
            model.UserId = user.UserId;
            model.UserPhone = user.Phone;
            model.UserAddress = user.Address;
            model.Location = user.Address;
            model.UserName = user.FullName;
            model.Active = true;
            model.Status = 0;
            model.FailedDeliveryMoney = 20000;
            model.TotalMoney = model.ShipCost + model.FailedDeliveryMoney;
            model.EstimatedDeliveryDate = DateTime.Now.AddDays(3);
            if (model.UploadImagesPackage != null)
            {
                model.ImagesPackages = hostUrl + "UploadFile/" + model.UploadImagesPackage.FileName;
            }
            else
            {
                model.ImagesPackages = string.Empty;
            }



            //add history
            IEnumerable<StatusOrder> lstStatus = await this.GetListStatus();

            foreach (var item in lstStatus)
            {
                if (model.Status == item.StatusInt)
                {
                    model.StatusDetail = item.Status;
                }
            };
            var mapOrder = _mapper.Map<Order>(model);

            await _orderRepository.Insert(mapOrder);
            await _orderRepository.Commit();

            //List<History> his = new List<History>()
            //{
            //    new History()
            //    {
            //        OrderId = mapOrder.OrderId,
            //        DriverId = mapOrder.DriverId,
            //        Reason = null,
            //        Status = model.Status,
            //        ChangeDate = DateTime.Now,
            //        OrderDate = DateTime.Now
            //    }
            //};

            //mapOrder.Histories = his;


            foreach (var item in model.ListOrderDetail)
            {
                if (item.UploadImagesProduct != null)
                {
                    item.ImagesProduct = hostUrl + "UploadFile/" + item.UploadImagesProduct.FileName;
                }
                else
                {
                    item.ImagesProduct = string.Empty;
                }

                item.OrderId = mapOrder.OrderId;
                var mapOrderDetail = _mapper.Map<OrderDetail>(item);

                await _orderDetailRepository.Insert(mapOrderDetail);
                await _orderDetailRepository.Commit();
            }

            return new ErrorMessage<OrderViewModel>
            {
                Code = 200,
                Message = "Inseart order success",
                Entity = model
            };
        }

        public async Task<ErrorMessage<OrderViewModel>> EditOrder(OrderViewModel model, int idOrder)
        {
            if (_contextAccessor.HttpContext is null)
            {
                return new ErrorMessage<OrderViewModel>
                {
                    Code = 500,
                    Message = "HttpContext is null"
                };
            }
            var email = _contextAccessor.HttpContext.User.FindFirst("Email")?.Value;
            if (email == null)
            {
                return new ErrorMessage<OrderViewModel>
                {
                    Code = 500,
                    Message = "Can't get email claim"
                };
            }

            Order order = await _orderRepository.GetById(idOrder);

            IEnumerable<OrderDetail> orderDetail = await _orderDetailRepository.GetByIdInObject(x => x.OrderId == idOrder);


            if (order == null || orderDetail == null)
            {
                return new ErrorMessage<OrderViewModel>
                {
                    Code = 500,
                    Message = "order && orderDetail not get data, check your idOrder"
                };
            }


            User user = await _userService.FindByEmail(email);

            List<IFormFile> files = new List<IFormFile>();

            if (model.ListOrderDetail != null)
            {
                foreach (var item in model.ListOrderDetail)
                {
                    if (item.UploadImagesProduct != null)
                    {
                        files.Add(item.UploadImagesProduct);
                    }
                }
            }


            if (model.UploadImagesPackage != null)
            {
                files.Add(model.UploadImagesPackage);
            }


            await Unilities.UploadMutipleImages(files);

            string hostUrl = "https://localhost:7130/";



            if (model.TotalGamPackage < 1000)
            {
                order.ShipCost = 30000;
            }
            order.OrderId = idOrder;
            order.UserId = user.UserId;
            order.UserPhone = user.Phone;
            order.UserAddress = user.Address;
            order.Location = user.Address;
            order.UserName = user.FullName;
            order.ReceiverAddress = model.ReceiverAddress;
            order.ReceiverDistrict = model.ReceiverDistrict;
            order.ReceiverName = model.ReceiverName;
            order.ReceiverPhone = model.ReceiverPhone;
            order.ReceiverWard = model.ReceiverWard;
            order.PaymentMethod = model.PaymentMethod;
            order.Location = model.Location;
            order.DriverAcceptAt = model.DriverAcceptAt;
            order.DriverId = model.DriverId;
            order.CompleteAt = model.CompleteAt;
            order.UserNote = model.UserNote;
            order.DeliveryNote = model.DeliveryNote;
            order.TotalGamPackage = model.TotalGamPackage;
            order.WidePackage = model.WidePackage.Value;
            order.HeightPackage = model.HeightPackage.Value;
            order.LongPackage = model.LongPackage.Value;
            order.TotalPriceProduct = model.TotalPriceProduct.Value;
            order.TotalCod = model.TotalCod;
            order.RequestSeeProduct = model.RequestSeeProduct;
            order.Active = true;
            order.Status = 0;
            order.FailedDeliveryMoney = 20000;
            order.TotalMoney = model.ShipCost + model.FailedDeliveryMoney;
            order.EstimatedDeliveryDate = DateTime.Now.AddDays(3);
            if (model.UploadImagesPackage != null)
            {
                order.ImagesPackages = hostUrl + "UploadFile/" + model.UploadImagesPackage.FileName;
            }
            else
            {
                order.ImagesPackages = order.ImagesPackages;
            }



            //add history
            IEnumerable<StatusOrder> lstStatus = await this.GetListStatus();

            foreach (var item in lstStatus)
            {
                if (model.Status == item.StatusInt)
                {
                    order.StatusDetail = item.Status;
                    break;
                }
            };
            //var mapOrder = _mapper.Map<Order>(model);

            _orderRepository.Update(order);
            await _orderRepository.Commit();

            //List<History> his = new List<History>()
            //{
            //    new History()
            //    {
            //        OrderId = idOrder,
            //        DriverId = order.DriverId,
            //        Reason = null,
            //        Status = model.Status,
            //        ChangeDate = DateTime.Now,
            //        OrderDate = DateTime.Now
            //    }
            //};

            //order.Histories = his;


            foreach (var item in model.ListOrderDetail)
            {
                foreach (var itemInOrderDetail in orderDetail)
                {
                    if (itemInOrderDetail.OrderDetailId == item.OrderDetailId)
                    {
                        if (item.UploadImagesProduct != null)
                        {
                            itemInOrderDetail.ImagesProduct = hostUrl + "UploadFile/" + item.UploadImagesProduct.FileName;
                        }
                        else
                        {
                            itemInOrderDetail.ImagesProduct = itemInOrderDetail.ImagesProduct;
                        }

                        itemInOrderDetail.OrderId = idOrder;
                        itemInOrderDetail.Gam = item.Gam;
                        itemInOrderDetail.Quantity = item.Quantity;
                        itemInOrderDetail.ProductId = item.ProductId;
                        itemInOrderDetail.ProductName = item.ProductName;

                        _orderDetailRepository.Update(itemInOrderDetail);
                        await _orderDetailRepository.Commit();
                    }

                }

            }

            return new ErrorMessage<OrderViewModel>
            {
                Code = 200,
                Message = "Inseart order success",
                Entity = model
            };
        }

        public async Task<ErrorMessage<OrderViewModel>> UploadFileExcel(IFormFile file)
        {
            if (_contextAccessor.HttpContext is null)
            {
                return new ErrorMessage<OrderViewModel>
                {
                    Code = 500,
                    Message = "HttpContext is null"
                };
            }
            var email = _contextAccessor.HttpContext.User.FindFirst("Email")?.Value;
            if (email == null)
            {
                return new ErrorMessage<OrderViewModel>
                {
                    Code = 500,
                    Message = "Can't get email claim"
                };
            }

            User user = await _userService.FindByEmail(email);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var filePath = await Unilities.UploadFileExcel(file);

            Application excel = new Application();
            Workbook wb = excel.Workbooks.Open(filePath);
            Worksheet ws = (Worksheet)wb.Sheets["Sheet1"];

            Excel.Range cell = ws.Range["A5"];
            string cellValue = cell.Value.ToString();

            Excel.Range xlRange = ws.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;


            Order order = new Order();
            OrderDetail orderDetail = new OrderDetail();

            IEnumerable<StatusOrder> lstStatus = await this.GetListStatus();

            //iterate over the rows and columns and print to the console as it appears in the file
            //excel is not zero based!!
            for (int i = 5; i <= rowCount; i++)
            {
                order.OrderId = 0;
                order.UserId = user.UserId;
                order.UserAddress = user.Address;
                order.UserName = user.FullName;
                order.UserPhone = user.Phone;
                order.Active = true;
                order.Status = 0;
                foreach (var item in lstStatus)
                {
                    if (order.Status == item.StatusInt)
                    {
                        order.StatusDetail = item.Status;
                    }
                };
                order.Location = user.Address;
                order.ReceiverName = xlRange.Cells[i, 1].Value.ToString();
                order.ReceiverPhone = xlRange.Cells[i, 2].Value.ToString();
                order.ReceiverAddress = xlRange.Cells[i, 3].Value.ToString();
                order.ReceiverDistrict = xlRange.Cells[i, 3].Value.ToString();
                order.ReceiverWard = xlRange.Cells[i, 3].Value.ToString();
                order.PaymentMethod = bool.Parse(xlRange.Cells[i, 4].Value.ToString());
                order.TotalCod = int.Parse(xlRange.Cells[i, 5].Value.ToString());
                order.RequestSeeProduct = bool.Parse(xlRange.Cells[i, 6].Value.ToString());
                order.TotalGamPackage = int.Parse(xlRange.Cells[i, 7].Value.ToString());
                order.LongPackage = int.Parse(xlRange.Cells[i, 8].Value.ToString());
                order.WidePackage = int.Parse(xlRange.Cells[i, 9].Value.ToString());
                order.HeightPackage = int.Parse(xlRange.Cells[i, 10].Value.ToString());
                order.TotalPriceProduct = int.Parse(xlRange.Cells[i, 11].Value.ToString());
                order.ShipCost = 30000;
                order.UserNote = xlRange.Cells[i, 13].Value.ToString();


                order.FailedDeliveryMoney = int.Parse(xlRange.Cells[i, 14].Value.ToString());

                if (order.FailedDeliveryMoney == 0)
                {
                    order.TotalMoney = order.ShipCost;
                }
                else
                {
                    order.TotalMoney = order.ShipCost + order.FailedDeliveryMoney;
                }


                await _orderRepository.Insert(order);
                await _orderRepository.Commit();

                orderDetail.OrderDetailId = 0;
                orderDetail.OrderId = order.OrderId;
                orderDetail.ProductName = xlRange.Cells[i, 12].Value.ToString();

                await _orderDetailRepository.Insert(orderDetail);
                await _orderDetailRepository.Commit();


            };


            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //rule of thumb for releasing com objects:
            //  never use two dots, all COM objects must be referenced and released individually
            //  ex: [somthing].[something].[something] is bad

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(ws);

            //close and release
            wb.Close();
            Marshal.ReleaseComObject(wb);

            //quit and release
            excel.Quit();
            Marshal.ReleaseComObject(excel);


            return new ErrorMessage<OrderViewModel>
            {
                Code = 200,
                Message = "Import data excel success!",
            };

        }

        public async Task<ErrorMessage<Order>> ChangeStatusOrder(/*int orderId, int status*/ ChangeOrderViewModel model)
        {
            try
            {
                if (_contextAccessor.HttpContext is null)
                {
                    return new ErrorMessage<Order>
                    {
                        Code = 500,
                        Message = "HttpContext is null"
                    };
                }
                var email = _contextAccessor.HttpContext.User.FindFirst("Email")?.Value;
                if (email == null)
                {
                    return new ErrorMessage<Order>
                    {
                        Code = 500,
                        Message = "Can't get email claim"
                    };
                }

                Driver driver = await _driverService.FindByEmail(email);

                Order order = await _orderRepository.GetSingleByConditionAsync(x => x.OrderId == model.OrderId);

                IEnumerable<StatusOrder> lstStatus = await this.GetListStatus();

                order.Status = model.Status;

                foreach (var item in lstStatus)
                {
                    if (order.Status == item.StatusInt)
                    {
                        order.StatusDetail = item.Status;
                    }
                };

                if (model.Status == 1)
                {
                    order.DriverId = driver.DriverId;
                    order.DriverAcceptAt = DateTime.UtcNow.ToLocalTime();
                    order.EstimatedDeliveryDate = DateTime.UtcNow.ToLocalTime().AddDays(3);
                    order.Status = model.Status;
                }

                if (model.Status == 3)
                {
                    order.DriverId = null;
                    order.DriverAcceptAt = null;
                    order.EstimatedDeliveryDate = null;
                    order.Status = 0;
                }

                if (model.Status == 2 || order.Status == 4)
                {
                    order.Active = false;
                    order.Status = model.Status;
                    await _historyService.InsertHistory(new History()
                    {
                        HistoryId = 0,
                        OrderId = order.OrderId,
                        Status = model.Status,
                        OrderDate = DateTime.UtcNow.ToLocalTime(),
                        ChangeDate = null,
                        DriverId = driver.DriverId,
                        Reason = model.Reason
                    });
                }

                if (model.Status == 5)
                {
                    order.CompleteAt = DateTime.UtcNow.ToLocalTime();
                    order.Status = 5;

                    await _historyService.InsertHistory(new History()
                    {
                        HistoryId = 0,
                        OrderId = order.OrderId,
                        Status = model.Status,
                        OrderDate = DateTime.UtcNow.ToLocalTime(),
                        ChangeDate = null,
                        DriverId = driver.DriverId,
                        Reason = model.Reason
                    });
                }

                order.Location = model.Location;


                //    List<History> his = new List<History>()
                //{
                //    new History()
                //    {
                //        OrderId = order.OrderId,
                //        DriverId = driver.DriverId,
                //        Reason = model.Reason,
                //        Status = order.Status,
                //        ChangeDate = DateTime.Now,
                //        OrderDate = DateTime.Now
                //    }

                //};

                //    order.Histories = his;

                _orderRepository.Update(order);
                await _orderRepository.Commit();


                return new ErrorMessage<Order>
                {
                    Code = 200,
                    Message = "Change status success",
                    Entity = order
                };
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ErrorMessage<Order>> DriverAcceptOrder(int orderId)
        {
            if (_contextAccessor.HttpContext is null)
            {
                return new ErrorMessage<Order>
                {
                    Code = 500,
                    Message = "HttpContext is null"
                };
            }
            var email = _contextAccessor.HttpContext.User.FindFirst("Email")?.Value;
            if (email == null)
            {
                return new ErrorMessage<Order>
                {
                    Code = 500,
                    Message = "Can't get email claim"
                };
            }

            Driver driver = await _driverService.FindByEmail(email);

            Order order = await _orderRepository.GetSingleByConditionAsync(x => x.OrderId == orderId);

            IEnumerable<StatusOrder> lstStatus = await this.GetListStatus();

            order.DriverId = driver.DriverId;
            order.DriverAcceptAt = DateTime.UtcNow.ToLocalTime();
            order.EstimatedDeliveryDate = DateTime.UtcNow.ToLocalTime().AddDays(3);
            order.Status = 1;

            foreach (var item in lstStatus)
            {
                if (order.Status == item.StatusInt)
                {
                    order.StatusDetail = item.Status;
                }
            };

            //List<History> his = new List<History>()
            //{
            //    new History()
            //    {
            //        OrderId = order.OrderId,
            //        DriverId = driver.DriverId,
            //        Reason = null,
            //        Status = order.Status,
            //        ChangeDate = DateTime.Now,
            //        OrderDate = DateTime.Now
            //    }

            //};
            //// luu vao bo nho dem xong se add hoac update
            //order.Histories = his;

            _orderRepository.Update(order);
            await _orderRepository.Commit();

            return new ErrorMessage<Order>
            {
                Code = 200,
                Message = "Accept order success",
                Entity = order

            };
        }

        //public async Task<ErrorMessage<Order>> DriverCancelOrder(int idOrder)
        //{
        //    if (_contextAccessor.HttpContext is null)
        //    {
        //        return new ErrorMessage<Order>
        //        {
        //            Code = 500,
        //            Message = "HttpContext is null"
        //        };
        //    }
        //    var email = _contextAccessor.HttpContext.User.FindFirst("Email")?.Value;
        //    if (email == null)
        //    {
        //        return new ErrorMessage<Order>
        //        {
        //            Code = 500,
        //            Message = "Can't get email claim"
        //        };
        //    }

        //    Driver driver = await _driverService.FindByEmail(email);

        //    Order order = await _orderRepository.GetSingleByConditionAsync(x => x.OrderId == model.OrderId);

        //    IEnumerable<StatusOrder> lstStatus = await this.GetListStatus();

        //    order.Active = false;
        //    order.DriverId = null;
        //    order.Status = model.Status;

        //    foreach (var item in lstStatus)
        //    {
        //        if (order.Status == item.StatusInt)
        //        {
        //            order.StatusDetail = item.Status;
        //        }
        //    };

        //    _orderRepository.Update(order);
        //    await _orderRepository.Commit();
        //}

        public async Task<IEnumerable<Order>> SearchOrder(string? text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                return await _orderRepository.GetListT(x => Unilities.RemoveUnicode(x.UserPhone.Trim()).Contains(Unilities.RemoveUnicode(text.Trim())) || Unilities.RemoveUnicode(x.ReceiverName.Trim()).Contains(Unilities.RemoveUnicode(text.Trim())));
            }
            return await _orderRepository.GetListT();
        }

        public async Task<IEnumerable<StatusOrder>> GetListStatus()
        {
            return await _db.StatusOrders.ToListAsync();
        }

        public async Task CancelOrderUser(int orderId)
        {
            var getOrder = await _orderRepository.GetById(orderId);
            if (getOrder != null)
            {
                getOrder.Status = 3;
                getOrder.Active = false;
                await _historyService.InsertHistory(new History()
                {
                    HistoryId = 0,
                    OrderId = orderId,
                    Status = 3,
                    OrderDate = DateTime.UtcNow.ToLocalTime(),
                    ChangeDate = null,
                    DriverId = null,
                    Reason = "Đặt nhầm"
                });
            }
            _orderRepository.Update(getOrder);
            await _orderRepository.Commit();
        }

    }

}
