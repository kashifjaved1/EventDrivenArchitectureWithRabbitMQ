using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// following code block registers the bus
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((ctx, cfg) =>
    {
        // host provided only and rest options left to default.
        cfg.Host("rabbitmq://localhost");

        // provided host, virtualHost, and rmq host configurations. [NOTE]: '/' is a default virtualHost and if you create one and want to use that replace '/' with your own virualHost.
        //cfg.Host("localhost", "/", h =>
        //{
        //    h.Username("guest");
        //    h.Password("guest");
        //});
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
