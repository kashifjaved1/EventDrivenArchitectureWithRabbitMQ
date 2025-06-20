using System.Text;
using System.Text.Json;
using EventDriven.Shared;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;

namespace EventDriven.OrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly IConnectionFactory _factory;

        public OrdersController(IConnectionFactory factory)
        {
            _factory = factory;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] string customerEmail)
        {
            var orderId = Guid.NewGuid();
            var orderEvent = new OrderCreated
            {
                OrderId = orderId,
                CustomerEmail = customerEmail
            };

            using var connection = await _factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            await channel.QueueDeclareAsync("order_created", durable: true, exclusive: false, autoDelete: false);

            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(orderEvent));
            await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "order_created", body: body);

            return Ok($"Order {orderId} created and event published.");
        }
    }
}
