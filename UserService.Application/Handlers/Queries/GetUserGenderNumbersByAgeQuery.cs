using MediatR;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Domain.Models.Constants;
using UserService.Domain.Responses;

namespace UserService.Application.Handlers.Queries
{
    public class GetUserGenderNumbersByAgeQuery : IRequest<IResponse<IEnumerable<IUserGenderNumberData>>>
    {
    }

    public class GetUserGenderNumbersByAgeQueryHandler : IRequestHandler<GetUserGenderNumbersByAgeQuery, IResponse<IEnumerable<IUserGenderNumberData>>>
    {
        private readonly IUserDataService _userDataService;

        public GetUserGenderNumbersByAgeQueryHandler(IUserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        public async Task<IResponse<IEnumerable<IUserGenderNumberData>>> Handle(GetUserGenderNumbersByAgeQuery request, CancellationToken cancellationToken)
        {
            var usersResponse = await _userDataService.Get(cancellationToken);
            if (!usersResponse.Success)
            {
                return Response<IEnumerable<IUserGenderNumberData>>.GetFailedResponse(usersResponse.Messages);
            }

            var groupedUsers = usersResponse.Data
                .GroupBy(g => g.Age)
                .OrderBy(o => o.Key)
                .Select(s => new UserGenderNumberData
                (
                    s.Key,
                    s.Count(c => c.Gender == UserConstants.Female),
                    s.Count(c => c.Gender == UserConstants.Male)
                ));

            if (!groupedUsers.Any())
            {
                return Response<IEnumerable<IUserGenderNumberData>>.GetFailedResponse(new List<string> { MessageConstants.NoUsersFound });
            }

            return Response<IEnumerable<IUserGenderNumberData>>.GetSuccessResponse(groupedUsers);
        }
    }
}