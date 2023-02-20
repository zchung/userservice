using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace UserService.Application.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection SetupApplication(this IServiceCollection services)
        {
            services.AddMediatR(config => config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
            return services;
        }
    }
}