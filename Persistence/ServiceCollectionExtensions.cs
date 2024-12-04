using Microsoft.Extensions.DependencyInjection;
using SharedKernel;

namespace Persistence;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterPersistence(this IServiceCollection services)
    {

        return services
             .AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>))
             .AddTransient<IUnitOfWork, UnitOfWork>();
    }
}

