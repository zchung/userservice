﻿using MediatR;
using UserService.Domain.Models;
using UserService.Domain.Responses;
using UserService.Infrastructure.Services.Interfaces;

namespace UserService.Application.Handlers.Queries
{
    public class GetUsersByAgeQuery : IRequest<IResponse<IEnumerable<IUser>>>
    {
        public int Age { get; }

        public GetUsersByAgeQuery(int age)
        {
            Age = age;
        }
    }

    public class GetUsersByAgeQueryHandler : IRequestHandler<GetUsersByAgeQuery, IResponse<IEnumerable<IUser>>>
    {
        private readonly IUserDataService _userDataService;

        public GetUsersByAgeQueryHandler(IUserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        public async Task<IResponse<IEnumerable<IUser>>> Handle(GetUsersByAgeQuery request, CancellationToken cancellationToken)
        {
            var users = await _userDataService.Get(cancellationToken);
            var result = users.Where(u => u.Age == request.Age).OrderBy(o => o.Last);

            return result.Any() ?
                Response<IEnumerable<IUser>>.GetSuccessResponse(result) :
                Response<IEnumerable<IUser>>.GetFailedResponse(new List<string> { "No users found." });
        }
    }
}