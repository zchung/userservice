using MediatR;
using UserService.Domain.Responses;
using UserService.Infrastructure.Services.Interfaces;

namespace UserService.Application.Handlers.Queries
{
    public class GetUsersQuery : IRequest<IUsersResponse>
    {
    }

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IUsersResponse>
    {
        private readonly IUserDataService _userDataService;

        public GetUsersQueryHandler(IUserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        public async Task<IUsersResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            // TODO: error handling
            return new UsersResponse(await _userDataService.Get(cancellationToken));
        }
    }
}