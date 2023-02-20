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

        public HttpUserDataService(IConfigurationService configurationService, IHttpClientFactory httpClientFactory, ILogger<HttpUserDataService> logger)
        {
            _configurationService = configurationService;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
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

                if (pollyResult.Outcome != OutcomeType.Successful)
                {
                    EventId eventId = new EventId(1, "HttpRequestException");
                    _logger.LogError(eventId, pollyResult.FinalException, MessageConstants.RetryErrorHttpRequest);
                    return Response<IEnumerable<IUser>>.GetFailedResponse(new List<string> { MessageConstants.RetryErrorHttpRequest });
                }

                var json = pollyResult.Result;
                var users = JsonConvert.DeserializeObject<IEnumerable<User>>(json);

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