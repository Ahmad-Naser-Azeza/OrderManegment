using MessageRecievers;
using RabbitMQ.Client;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton(sp =>
{
    return new ConnectionFactory
    {
        HostName = "localhost", // Replace with your RabbitMQ host
        UserName = "guest",     // Replace with your RabbitMQ username
        Password = "guest"      // Replace with your RabbitMQ password
    };
});
// Register RabbitMQ receiver as a dependency
builder.Services.AddSingleton(typeof(IRabbitMQReceiver<>), typeof(RabbitMQReceiver<>));

// Register your custom message processing service
builder.Services.AddSingleton<OrderReceiverService>();

// Add the worker service
builder.Services.AddHostedService<OrderMessageReceiver>();

// Build and run the host
var host = builder.Build();
host.Run();
