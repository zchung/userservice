using MediatR;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Domain.Models.Constants;
using UserService.Domain.Responses;

namespace UserService.Application.Handlers.Queries
{
    public class GetUserGenderNumbersByAgeQuery : IRequest<IResponse<IEnumerable<IUserGenderNumberData>>>
    {
        public int? PageSize { get; set; }

        public int? PageNumber { get; set; }

        public GetUserGenderNumbersByAgeQuery(int? pageSize = null, int? pageNumber = null)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
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

            if (!usersResponse.Data.Any())
            {
                return Response<IEnumerable<IUserGenderNumberData>>.GetFailedResponse(new List<string> { MessageConstants.NoUsersFound });
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

            if (request.PageNumber.HasValue &&
                request.PageNumber.Value > 0 &&
                request.PageSize.HasValue &&
                request.PageSize.Value > 0)
            {
                groupedUsers = groupedUsers.Skip(request.PageNumber.Value - 1).Take(request.PageSize.Value);
            }

            return Response<IEnumerable<IUserGenderNumberData>>.GetSuccessResponse(groupedUsers);
        }
    }
}