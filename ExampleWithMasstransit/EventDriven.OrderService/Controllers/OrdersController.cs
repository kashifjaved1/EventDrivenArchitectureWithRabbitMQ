using EventDriven.Shared;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace EventDriven.OrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IPublishEndpoint _publish;

        public OrdersController(IPublishEndpoint publish) => _publish = publish;

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] string customerEmail)
        {
            var orderId = Guid.NewGuid();
            await _publish.Publish<OrderCreated>(new
            {
                OrderId = orderId,
                CustomerEmail = customerEmail
            });

            return Ok($"Order {orderId} created.");
        }
    }
}
