using EventDriven.EmailService.Consumers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedConsumer>();

    // In MassTransit, the bus is registered and automatically started via services.AddMassTransit(...). You typically don’t see or touch IBusControl or IBus manually unless you're doing advanced hosting scenarios.
    // What this code is doing:
    // 1. Initializes IBusControl internally
    // 2. Connects to RabbitMQ
    // 3. Automatically binds your consumers
    // 4. Starts the bus when the app starts

    x.UsingRabbitMq((ctx, cfg) =>
    {
        // host provided only and rest options left to default.
        cfg.Host("rabbitmq://localhost");

        // provided host, virtualHost, and rmq host configurations.
        //cfg.Host("localhost", "/", h =>
        //{
        //    h.Username("guest");
        //    h.Password("guest");
        //});

        cfg.ConfigureEndpoints(ctx); // This'll make sure that above provided consumer will get data when queue got message from publisher by acting as receiving endpoint.
    });
});

var app = builder.Build();
app.Run();
