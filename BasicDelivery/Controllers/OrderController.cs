using BasicDelivery.Domain.ViewModel;
using BasicDelivery.Service.OrderService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BasicDelivery.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpGet("get-all-order-user")]
        public async Task<IActionResult> GetAllOrderUser()
        {
            try
            {
                return Ok(await _orderService.GetListOrderUser());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }

        [HttpGet("get-all-order-deliver")]
        public async Task<IActionResult> GetAllOrderDeliver()
        {
            try
            {
                return Ok(await _orderService.GetListOrderUserDeliver());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }

        [HttpGet("get-all-order-created")]
        public async Task<IActionResult> GetAllOrderCreated()
        {
            try
            {
                return Ok(await _orderService.GetListOrderUserCreated());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }

        [HttpGet("get-all-order-completed")]
        public async Task<IActionResult> GetAllOrderCompleted()
        {
            try
            {
                return Ok(await _orderService.GetListOrderUserCompleted());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }

        [HttpGet("get-all-order-cancel")]
        public async Task<IActionResult> GetAllOrderCancel()
        {
            try
            {
                return Ok(await _orderService.GetListOrderUserCancel());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }

        [HttpGet("get-all-order-lost")]
        public async Task<IActionResult> GetAllOrderLost()
        {
            try
            {
                return Ok(await _orderService.GetListOrderUserLost());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }

        [HttpGet("get-all-order-fail")]
        public async Task<IActionResult> GetAllOrderFail()
        {
            try
            {
                return Ok(await _orderService.GetListOrderUserFail());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }


        // driver
        [HttpGet("get-list-order-driver")]
        public async Task<IActionResult> GetListOrderDriver()
        {
            try
            {
                return Ok(await _orderService.GetListOrderDriver());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }

        [HttpGet("get-all-order-driver")]
        public async Task<IActionResult> GetAllOrderDriver()
        {
            try
            {
                return Ok(await _orderService.GetAllOrderDriver());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }

        [HttpGet("get-list-order-driver-completed")]
        public async Task<IActionResult> GetListOrderDriverCompleted()
        {
            try
            {
                return Ok(await _orderService.GetListOrderDriverCompleted());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }

        [HttpGet("get-order-driver-accepted")]
        public async Task<IActionResult> GetAllOrderDriverAccepted()
        {
            try
            {
                return Ok(await _orderService.GetListOrderDriverAccepted());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }


        [HttpGet("{id:int}/order-detail")]
        //[AllowAnonymous]
        public async Task<IActionResult> GetDetailOrder(int id = 0)
        {
            try
            {
                if (id == 0) return BadRequest("Id required > 0");
                var getDetailOrder = await _orderService.GetDetailOrderById(id);
                if (getDetailOrder != null)
                {
                    return Ok(getDetailOrder);
                }
                return BadRequest("Order not in database check your ID!");


            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }


        [HttpPost("insert-order")]
        public async Task<IActionResult> InsertOrder(/*[FromForm] */OrderViewModel model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest();
                }

                return Ok(await _orderService.InsertOrder(model));

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }

        [HttpPost("{id:int}/order-edit")]
        public async Task<IActionResult> EditOrder(OrderViewModel model, int id)
        {
            try
            {
                return Ok(await _orderService.EditOrder(model, id));
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }

        [HttpGet("{idOrder:int}/get-order")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrder(int idOrder)
        {
            try
            {
                return Ok(await _orderService.GetDetailOrderById(idOrder));
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }

        [HttpPost("excel-order")]
        public async Task<IActionResult> ImportExcelDataOrder(IFormFile file)
        {
            try
            {
                if (file == null) return BadRequest("File is null.");
                return Ok(await _orderService.UploadFileExcel(file));
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }


        [HttpPut("{id:int}/accept-order")]
        public async Task<IActionResult> AcceptOrder(int id = 0)
        {
            try
            {
                if (id == 0) return BadRequest("ID required > 0");
                return Ok(await _orderService.DriverAcceptOrder(id));
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }

        [HttpPut("change-order")]
        public async Task<IActionResult> ChangeStatusOrder(/*int orderId, int status, string? reason*/ChangeOrderViewModel model)
        {
            try
            {
                if (model.OrderId == 0 && model.Status == 0) return BadRequest("Order and status not = 0!");
                return Ok(await _orderService.ChangeStatusOrder(model));
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }

        [HttpGet("search-order")]
        [AllowAnonymous]
        public async Task<IActionResult> SearchOrder(string? text)
        {
            try
            {
                var searchOrder = await _orderService.SearchOrder(text);
                if (searchOrder == null)
                {
                    return BadRequest("Order isn't in db");
                }
                return Ok(searchOrder);

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }

        [HttpGet("get-status")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStatus()
        {
            try
            {
                return Ok(await _orderService.GetListStatus());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }

        [HttpPut("{id:int}/cancel-order")]
        public async Task<IActionResult> CancelOrder(int id = 0)
        {
            try
            {
                if (id == 0) return BadRequest("Order Id is failed");
                await _orderService.CancelOrderUser(id);
                return Ok("Delete Success");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    // Log the inner exception
                    return BadRequest($"Inner Exception: {ex.InnerException.Message}");
                }
                // Log the outer exception
                return BadRequest($"Outer Exception: {ex.Message}");
            }
        }


    }
}
