using Microsoft.AspNetCore.Mvc;
using OrderSystem.SDK.Contract;
using OrderSystem.SDK.RequestModel;

namespace OrderSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderSystemController : ControllerBase
    {
        private readonly ILogger<OrderSystemController> _logger;
        private readonly IOrderService _orderService;
        public OrderSystemController(ILogger<OrderSystemController> logger, IOrderService orderService)
        {
            _logger = logger;
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] GetOrderRequest request)
        {
            var response = await _orderService.GetAllOrder(request);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] CreateOrderRequest request)
        {
            var response = await _orderService.CreateOrder(request);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> EditOrder([FromBody] EditOrderRequest request)
        {
            var response = await _orderService.ModifyOrder(request);
            if(response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var response = await _orderService.DeleteOrder(orderId);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPatch("{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            var response = await _orderService.CancelOrder(orderId);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}
