using Microsoft.Extensions.DependencyInjection;
using UserService.Application.Extensions;
using UserService.Domain.Providers;
using UserService.Infrastructure.Extensions;
using UserService.Providers;

namespace UserService.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<UserWorker>();
            services.AddTransient<IConfigurationService, NetCoreConfigurationService>();
            services.AddHttpClient();
            services.RegisterInfrastructure();
            services.AddLogging();
            services.SetupApplication();
            services.AddDistributedMemoryCache();

            return services;
        }
    }
}