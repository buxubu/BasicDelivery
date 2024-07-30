using BasicDelivery.Service.HistoryService;
using BasicDelivery.Service.OrderService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BasicDelivery.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {
        private readonly IHistoryService _historyService;
        public HistoryController(IHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpGet("get-all-history")]
        public async Task<IActionResult> GetAllHistory()
        {
            try
            {
                return Ok(await _historyService.GetHistories());
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

        [HttpGet("get-all-history-driver")]
        public async Task<IActionResult> GetAllHistoryDriver()
        {
            try
            {
                return Ok(await _historyService.GetHistoriesDriver());
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
