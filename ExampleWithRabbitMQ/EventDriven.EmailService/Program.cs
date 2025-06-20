using System.Text;
using System.Text.Json;
using EventDriven.Shared;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "guest",
    Password = "guest"
};

using var connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync("order_created", durable: true, exclusive: false, autoDelete: false);

var consumer = new AsyncEventingBasicConsumer(channel);
consumer.ReceivedAsync += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    var order = JsonSerializer.Deserialize<OrderCreated>(message);
    Console.WriteLine($"Email sent to {order?.CustomerEmail} for order {order?.OrderId}");
    return Task.CompletedTask;
};

await channel.BasicConsumeAsync(queue: "order_created",
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine("Waiting for messages. Press [enter] to exit.");
Console.ReadLine();
