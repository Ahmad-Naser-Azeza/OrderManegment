using Kernel.Contract;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace SharedKernel;
public static class ServiceCollectionExtensions
{
    //public static void AddSharedRegistrations(this IServiceCollection services, Assembly assembly = null)
    //{
    //    services.AddAuthentications();
    //    services.AddApiKeyAuthentication();
    //    services.AddAuthorizationPermissions();
    //    services.AddSingleton<IHttpContext, HttpContext>();
    //    services.AddScoped<Dispatcher>();
    //    DomainEvents.RegisterHandlers(assembly, services);
    //    services.AddScoped<IDomainEvents, DomainEvents>();
    //    services.AddCQRSHandlers(assembly);
    //    services.AddHttpClient();
    //    services.AddHttpContextAccessor();
    //}

    //private static void AddAuthentications(this IServiceCollection services)
    //{
    //    services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    //        .AddJwtBearer();
    //}

    //private static void AddAuthorizationPermissions(this IServiceCollection services)
    //{
    //    services.AddSingleton<IIdentitySettings, IdentitySettings>();
    //    services.AddSingleton<IJwtManager, JwtManager>();
    //}
   
    public static void AddDbContext<TDbContext>(this IServiceCollection services,string ConnectionString) where TDbContext : DbContext
    {
        services.AddDbContext<TDbContext>((serviceProvider, options) => { 

                options.UseSqlServer(ConnectionString);         
        });
        services.AddScoped<DbContext>(provider => provider.GetRequiredService<TDbContext>());
    }

    //public static void MigrateDb<TDbContext>(this IApplicationBuilder app) where TDbContext : DbContext
    //{
    //    using IServiceScope serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
    //    serviceScope.ServiceProvider.GetRequiredService<TDbContext>().Database.Migrate();
    //}

   

    //private static void AddApiKeyAuthentication(this IServiceCollection services)
    //{
    //    services.AddSingleton<ApiKeyAuthorizationFilter>();
    //    services.AddSingleton<IApiKeyValidator, ApiKeyValidator>();
    //}
}
