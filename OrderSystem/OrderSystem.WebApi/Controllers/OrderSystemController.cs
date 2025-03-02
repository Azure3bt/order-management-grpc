using Microsoft.AspNetCore.Mvc;
using OrderSystem.SDK.Contract;
using OrderSystem.SDK.RequestModel;

namespace OrderSystem.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderSystemController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderSystemController(ILogger<OrderSystemController> logger, IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] GetOrderRequest request, CancellationToken cancellationToken)
        {
            var response = await _orderService.GetAllOrder(request, cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddOrder([FromBody] CreateOrderRequest request, CancellationToken cancellationToken)
        {
            var response = await _orderService.CreateOrder(request, cancellationToken);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> EditOrder([FromBody] EditOrderRequest request, CancellationToken cancellationToken)
        {
            var response = await _orderService.ModifyOrder(request, cancellationToken);
            if(response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId, CancellationToken cancellationToken)
        {
            var response = await _orderService.DeleteOrder(orderId, cancellationToken);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPatch("{orderId}")]
        public async Task<IActionResult> CancelOrder(int orderId, CancellationToken cancellationToken)
        {
            var response = await _orderService.CancelOrder(orderId, cancellationToken);
            if (response == null)
            {
                return NotFound();
            }
            return Ok(response);
        }
    }
}
