using Microsoft.Extensions.Configuration;
using UserService.Domain.Providers;

namespace UserService.Providers
{
    public class NetCoreConfigurationService : IConfigurationService
    {
        private readonly IConfiguration _configuration;

        public NetCoreConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<T> GetValue<T>(string key, T defaultValue)
        {
            var value = _configuration.GetValue<T>(key);
            return Task.FromResult(value ?? defaultValue);
        }
    }
}