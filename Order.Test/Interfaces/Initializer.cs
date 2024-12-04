using CoreOps.CourierRouteManagement.Application;
using Kernel.Contract;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Order.Infrastructure;
using Order.Test.Factory;
using Persistence;
using SharedKernel;

namespace CoreOps.Order.Tests
{
    public class Initializer : IClassFixture<OrdersControllerFactory>
    {        
        private readonly IMediator _mediator;                       
        private readonly Dispatcher _dispatcher;
        private readonly IConfiguration _configuration;
        
        public Initializer()
        {           
            var services = new ServiceCollection();
            IConfiguration configuration = new ConfigurationManager().AddJsonFile("appsettings.json").Build();
            var connectionString = configuration.GetConnectionString("DB");
            services.RegisterPersistence();
            services.AddSingleton<IHttpContext, SharedKernel.HttpContext>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IDomainEvents, DomainEvents>();
            services.AddLogging(logging => logging.AddConsole());
            services.AddDbContext<OrderDbContext>(connectionString);
            services.AddAuthorization();
            services.AddScoped<IMediator, Mediator>();
            services.AddControllers();
            services.AddOrderModuleCore();
            services.AddEndpointsApiExplorer();
            services.AddScoped<Dispatcher>();

            var provider = services.BuildServiceProvider();

            _mediator = provider.GetRequiredService<IMediator>();
            _dispatcher = provider.GetRequiredService<Dispatcher>();
            _configuration = configuration;
        }
        public IConfiguration GetConfigurations()
        {
            return _configuration;
        }
        public Dispatcher GetDispatcher()
        {
            return _dispatcher;
        }   
        public IMediator GetMediator()
        {
            return _mediator;
        }
    }

}
