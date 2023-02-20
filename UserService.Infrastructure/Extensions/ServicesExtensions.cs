using Microsoft.Extensions.DependencyInjection;
using UserService.Domain.Interfaces;
using UserService.Infrastructure.Services;

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