using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserService.Helpers;

internal class Program
{
    private static void Main(string[] args)
    {
        // Setup Host
        var host = HostHelpers.CreateDefaultBuilder().Build();

        // Invoke Worker
        using IServiceScope serviceScope = host.Services.CreateScope();
        IServiceProvider provider = serviceScope.ServiceProvider;
        var workerInstance = provider.GetRequiredService<UserWorker>();
        workerInstance.WriteUserFullNameById(42);
        workerInstance.WriteUsersByAge(23);
        workerInstance.WriteUserGenderNumbersByAge();

        host.Run();
    }
}