using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCommercial.Services;

namespace MiniCommercial.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public DashboardController(IOrderService orderService) => _orderService = orderService;

        [HttpGet]
        public async Task<IActionResult> GetStats([FromQuery] DateTime? start, [FromQuery] DateTime? end)
        {
            return Ok(await _orderService.GetDashboardStatsAsync(start, end));
        }
    }
}
