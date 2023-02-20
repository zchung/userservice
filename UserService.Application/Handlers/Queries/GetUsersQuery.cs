using MediatR;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Domain.Responses;

namespace UserService.Application.Handlers.Queries
{
    public class GetUsersQuery : IRequest<IResponse<IEnumerable<IUser>>>
    {
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IResponse<IEnumerable<IUser>>>
    {
        private readonly IUserDataService _userDataService;

        public GetUsersQueryHandler(IUserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        public async Task<IResponse<IEnumerable<IUser>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            // TODO: error handling
            return Response<IEnumerable<IUser>>.GetSuccessResponse(await _userDataService.Get(cancellationToken));
        }
    }
}