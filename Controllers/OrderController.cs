using Microsoft.AspNetCore.Mvc;
using OrderBookingSystem.HelperClasses;
using OrderBookingSystem.IServices;
using OrderBookingSystem.Models.ViewModels;

namespace OrderBookingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("Order")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<IActionResult> BookOrder([FromBody] OrderBookingRequest request)
        {
            return Ok(await _orderService.BookOrderAsync(request));
        }
    }

}
