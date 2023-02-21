using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Domain.Models.Constants;
using UserService.Domain.Providers;
using UserService.Domain.Responses;

namespace UserService.Infrastructure.Services
{
    public class HttpUserDataService : IUserDataService
    {
        private readonly IConfigurationService _configurationService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<HttpUserDataService> _logger;
        private readonly IDistributedCache _cache;

        public HttpUserDataService(IConfigurationService configurationService, IHttpClientFactory httpClientFactory, ILogger<HttpUserDataService> logger, IDistributedCache cache)
        {
            _configurationService = configurationService;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _cache = cache;
        }

        public async Task<IResponse<IEnumerable<IUser>>> Get(CancellationToken cancellationToken)
        {
            try
            {
                var usersDataUrl = await _configurationService.GetValue<string>(SettingsConstants.USERS_DATA_URL_KEY, SettingsConstants.USERS_DATA_URL_DEFAULT);

                var httpClient = _httpClientFactory.CreateClient();

                var pollyResult = await Policy.Handle<HttpRequestException>()
                    .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
                    .ExecuteAndCaptureAsync(() => httpClient.GetStringAsync(usersDataUrl, cancellationToken));
                string json = string.Empty;
                if (pollyResult.Outcome != OutcomeType.Successful)
                {
                    EventId eventId = new EventId(1, "HttpRequestException");
                    _logger.LogError(eventId, pollyResult.FinalException, MessageConstants.RetryErrorHttpRequest);
                    // try to get last cached data
                    json = await _cache.GetStringAsync(CacheConstants.USER_DATA_KEY, cancellationToken);

                    if (string.IsNullOrWhiteSpace(json))
                    {
                        return Response<IEnumerable<IUser>>.GetFailedResponse(new List<string> { MessageConstants.RetryErrorHttpRequest });
                    }
                }
                else
                {
                    json = pollyResult.Result;
                }
                var users = JsonConvert.DeserializeObject<IEnumerable<User>>(json);

                await _cache.SetStringAsync(CacheConstants.USER_DATA_KEY, json, cancellationToken);

                if (users == null)
                {
                    return Response<IEnumerable<IUser>>.GetFailedResponse(new List<string> { MessageConstants.NullUsersHttpRequest });
                }

                return Response<IEnumerable<IUser>>.GetSuccessResponse(users);
            }
            catch (Exception ex)
            {
                EventId eventId = new EventId(2, "Exception");
                _logger.LogError(eventId, ex, MessageConstants.ErrorGettingUsersHttpRequest);
                return Response<IEnumerable<IUser>>.GetFailedResponse(new List<string> { MessageConstants.ErrorGettingUsersHttpRequest });
            }
        }
    }
}