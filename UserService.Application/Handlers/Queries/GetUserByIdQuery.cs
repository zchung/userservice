using MediatR;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Domain.Responses;

namespace UserService.Application.Handlers.Queries
{
    public class GetUserByIdQuery : IRequest<IResponse<IUser>>
    {
        public int Id { get; }

        public GetUserByIdQuery(int id)
        {
            Id = id;
        }
    }

    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, IResponse<IUser>>
    {
        private readonly IUserDataService _userDataService;

        public GetUserByIdQueryHandler(IUserDataService userDataService)
        {
            _userDataService = userDataService;
        }

        public async Task<IResponse<IUser>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var users = await _userDataService.Get(cancellationToken);

            var filteredUser = users.FirstOrDefault(u => u.Id == request.Id);

            return filteredUser != null ?
                Response<IUser>.GetSuccessResponse(filteredUser) :
                Response<IUser>.GetFailedResponse(new List<string> { "No user by that Id" });
        }
    }
}