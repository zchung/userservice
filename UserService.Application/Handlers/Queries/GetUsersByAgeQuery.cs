using MediatR;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Domain.Models.Constants;
using UserService.Domain.Responses;

namespace UserService.Application.Handlers.Queries
{
    public class GetUsersByAgeQuery : IRequest<IResponse<IEnumerable<IUser>>>
    {
        public int Age { get; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }

        public GetUsersByAgeQuery(int age, int? pageSize = null, int? pageNumber = null)
        {
            Age = age;
            PageSize = pageSize;
            PageNumber = pageNumber;
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
            var usersResponse = await _userDataService.Get(cancellationToken);
            if (!usersResponse.Success)
            {
                return usersResponse;
            }
            var result = usersResponse.Data.Where(u => u.Age == request.Age).OrderBy(o => o.Last).AsEnumerable();

            if (request.PageNumber.HasValue &&
                request.PageNumber.Value > 0 &&
                request.PageSize.HasValue &&
                request.PageSize.Value > 0)
            {
                result = result.Skip(request.PageNumber.Value - 1).Take(request.PageSize.Value);
            }

            return result.Any() ?
                Response<IEnumerable<IUser>>.GetSuccessResponse(result) :
                Response<IEnumerable<IUser>>.GetFailedResponse(new List<string> { MessageConstants.NoUsersFound });
        }
    }
}