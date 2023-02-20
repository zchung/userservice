using Newtonsoft.Json;
using UserService.Domain.Models;
using UserService.Domain.Models.Constants;
using UserService.Domain.Providers;
using UserService.Infrastructure.Services.Interfaces;

namespace UserService.Infrastructure.Services
{
    public class HttpUserDataService : IUserDataService
    {
        private readonly IConfigurationService _configurationService;
        private readonly IHttpClientFactory _httpClientFactory;

        public HttpUserDataService(IConfigurationService configurationService, IHttpClientFactory httpClientFactory)
        {
            _configurationService = configurationService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IEnumerable<IUser>> Get(CancellationToken cancellationToken)
        {
            // TODO: add polly
            // TODO: add caching
            var usersDataUrl = await _configurationService.GetValue<string>(SettingsConstants.USERS_DATA_URL_KEY, SettingsConstants.USERS_DATA_URL_DEFAULT);

            var httpClient = _httpClientFactory.CreateClient();

            var json = await httpClient.GetStringAsync(usersDataUrl, cancellationToken);

            var users = JsonConvert.DeserializeObject<IEnumerable<User>>(json);

            return users;
        }
    }
}