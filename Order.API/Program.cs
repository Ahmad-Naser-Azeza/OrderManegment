using CoreOps.CourierRouteManagement.Application;
using Kernal;
using Kernel.Contract;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Order.Infrastructure;
using Persistence;
using RabbitMQ.Client;
using SharedKernel;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);                
        var connectionString = builder.Configuration.GetConnectionString("DB");
        builder.Services.RegisterPersistence();
        builder.Services.AddSingleton<IHttpContext, SharedKernel.HttpContext>();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        builder.Services.AddScoped<IDomainEvents, DomainEvents>();
        builder.Services.AddDbContext<OrderDbContext>(connectionString);
        builder.Services.AddAuthorization();
        builder.Services.AddScoped<IMediator, Mediator>();     
        builder.Services.AddControllers();
        builder.Services.AddOrderModuleCore();        
        builder.Services.AddEndpointsApiExplorer();        
        builder.Services.AddSwaggerGen(c =>
        {
            // Define the Bearer scheme for Swagger UI
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            // Add the security requirement to the Swagger document
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] {}
            }
        });
        });

        builder.Services.AddSingleton(sp =>
        {
            return new ConnectionFactory
            {
                HostName = "localhost", // Replace with your RabbitMQ host
                UserName = "guest",     // Replace with your RabbitMQ username
                Password = "guest"      // Replace with your RabbitMQ password
            };
        });
        // Register RabbitMQSender
        builder.Services.AddSingleton(typeof(IRabbitMQSender<>), typeof(RabbitMQSender<>));
        // Register the generic RabbitMQReceiver
        builder.Services.AddSingleton(typeof(IRabbitMQReceiver<>), typeof(RabbitMQReceiver<>));


        builder.Services.AddScoped<Dispatcher>();       

        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();        
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<OrderDbContext>();
            dbContext.Database.Migrate();
        }        
        app.UseMiddleware<GlobalExceptionHandlerMiddleware>();        
        app.UseMiddleware<JwtMiddleware>();        
        app.MapControllers();
        app.Run();
    }
}
