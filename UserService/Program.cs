using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Text.Json;
using UserService.Application.Handlers.Queries;
using UserService.Domain.Responses;
using UserService.Extensions;

internal class Program
{
    private static void Main(string[] args)
    {
        // Setup Host
        var host = CreateDefaultBuilder().Build();

        // Invoke Worker
        using IServiceScope serviceScope = host.Services.CreateScope();
        IServiceProvider provider = serviceScope.ServiceProvider;
        var workerInstance = provider.GetRequiredService<Worker>();
        workerInstance.DoWork();

        host.Run();
    }

    private static IHostBuilder CreateDefaultBuilder()
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

// Worker.cs
internal class Worker
{
    private readonly IMediator _mediator;

    public Worker(IMediator mediator)
    {
        _mediator = mediator;
    }

    public void DoWork()
    {
        Task<IUsersResponse> response = _mediator.Send(new GetUsersQuery());
        Console.WriteLine(JsonSerializer.Serialize(response.Result, new JsonSerializerOptions { WriteIndented = true }));
    }
}