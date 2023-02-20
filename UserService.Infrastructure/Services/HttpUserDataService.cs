﻿using Newtonsoft.Json;
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

        public HttpUserDataService(IConfigurationService configurationService, IHttpClientFactory httpClientFactory)
        {
            _configurationService = configurationService;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IResponse<IEnumerable<IUser>>> Get(CancellationToken cancellationToken)
        {
            try
            {
                var usersDataUrl = await _configurationService.GetValue<string>(SettingsConstants.USERS_DATA_URL_KEY, SettingsConstants.USERS_DATA_URL_DEFAULT);

                var httpClient = _httpClientFactory.CreateClient();

                var json = await httpClient.GetStringAsync(usersDataUrl, cancellationToken);

                var users = JsonConvert.DeserializeObject<IEnumerable<User>>(json);

                if (users == null)
                {
                    return Response<IEnumerable<IUser>>.GetFailedResponse(new List<string> { MessageConstants.NullUsersHttpRequest });
                }

                return Response<IEnumerable<IUser>>.GetSuccessResponse(users);
            }
            catch (Exception ex)
            {
                // log exception
                return Response<IEnumerable<IUser>>.GetFailedResponse(new List<string> { MessageConstants.ErrorGettingUsersHttpRequest });
            }
        }
    }
}