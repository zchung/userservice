using Microsoft.Extensions.DependencyInjection;
using UserService.Infrastructure.Services;
using UserService.Infrastructure.Services.Interfaces;

namespace UserService.Infrastructure.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
        {
            services.AddTransient<IUserDataService, HttpUserDataService>();
            return services;
        }
    }
}