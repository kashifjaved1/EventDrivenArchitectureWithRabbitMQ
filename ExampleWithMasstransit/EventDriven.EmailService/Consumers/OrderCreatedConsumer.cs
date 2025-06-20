using EventDriven.Shared;
using MassTransit;

namespace EventDriven.EmailService.Consumers
{
    public class OrderCreatedConsumer : IConsumer<OrderCreated>
    {
        public Task Consume(ConsumeContext<OrderCreated> context)
        {
            var msg = context.Message;
            Console.WriteLine($"Sending email to: {msg.CustomerEmail} for Order: {msg.OrderId}");
            return Task.CompletedTask;
        }
    }
}
