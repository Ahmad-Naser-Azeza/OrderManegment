using MediatorCoordinator;
using Microsoft.Extensions.DependencyInjection;

using SharedKernel;
using System.Reflection;

namespace CoreOps.CourierRouteManagement.Application;

public static class OrderExtensions
{
    public static IServiceCollection AddOrderModuleCore(this IServiceCollection services)
    {        
        services.AddMediatorCoordinator([typeof(OrderExtensions).Assembly]);
        services.AddCQRSHandlers(typeof(OrderExtensions).Assembly);
        return services;
    }
    private static IServiceCollection AddCQRSHandlers(this IServiceCollection services, Assembly assembly)
    {
        var assemblyTypes = assembly.GetTypes();
        foreach (var type in assemblyTypes)
        {
            var handlerInterfaces = type.GetInterfaces()
                .Where(SharedKernel.Extensions.IsHandlerInterface)
                .ToList();

            if (handlerInterfaces.Any())
            {
                var handlerFactory = new HandlerFactory(type);
                foreach (var interfaceType in handlerInterfaces)
                {
                    services.AddTransient(interfaceType, provider => handlerFactory.Create(provider, interfaceType));
                }
            }
        }
        return services;
    }
}