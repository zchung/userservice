using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using UserService.Extensions;

namespace UserService.Helpers
{
    public class HostHelpers
    {
        public static IHostBuilder CreateDefaultBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(app =>
                {
                    app.AddJsonFile("appsettings.json");
                })
                .ConfigureServices(services =>
                {
                    services.RegisterServices();
                });
        }
    }
}