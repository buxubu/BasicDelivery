using BasicDelivery.Domain.Entities;
using BasicDelivery.Domain.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasicDelivery.Service.OrderService
{
    public interface IOrderService
    {
        Task<ErrorMessage<Order>> ChangeStatusOrder(/*int orderId, int status*/ ChangeOrderViewModel model);
        Task<ErrorMessage<Order>> DriverAcceptOrder(int orderId);
        Task<ErrorMessage<OrderViewModel>> EditOrder(OrderViewModel model, int idOrder);
        Task<Order> GetDetailOrderById(int id);
        Task<IEnumerable<Order>> GetListOrderDriver();
        Task<IEnumerable<Order>> GetListOrderDriverAccepted();
        Task<IEnumerable<Order>> GetListOrderUserCreated();
        Task<IEnumerable<Order>> GetListOrderUser();
        Task<IEnumerable<StatusOrder>> GetListStatus();
        Task<ErrorMessage<OrderViewModel>> InsertOrder([FromForm] OrderViewModel model);
        Task<IEnumerable<Order>> SearchOrder(string? text);
        Task<ErrorMessage<OrderViewModel>> UploadFileExcel(IFormFile file);
        Task<IEnumerable<Order>> GetListOrderUserCompleted();
        Task<IEnumerable<Order>> GetListOrderUserCancel();
        Task<IEnumerable<Order>> GetListOrderUserLost();
        Task<IEnumerable<Order>> GetListOrderUserFail();
        Task<IEnumerable<Order>> GetListOrderUserDeliver();
        Task CancelOrderUser(int orderId);
        Task<IEnumerable<Order>> GetAllOrderDriver();
        Task<IEnumerable<Order>> GetListOrderDriverCompleted();
    }
}