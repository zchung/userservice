using MediatR;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Domain.Models.Constants;
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
            var usersResponse = await _userDataService.Get(cancellationToken);
            if (!usersResponse.Success)
            {
                return Response<IUser>.GetFailedResponse(usersResponse.Messages);
            }

            var filteredUser = usersResponse.Data.FirstOrDefault(u => u.Id == request.Id);

            return filteredUser != null ?
                Response<IUser>.GetSuccessResponse(filteredUser) :
                Response<IUser>.GetFailedResponse(new List<string> { MessageConstants.NoUserByThatId });

            // if the sample data was using unique ids this would be more efficient to search the list.
            //var userDictionary = usersResponse.Data.ToDictionary(d => d.Id); // convert to dictionary for improved performance

            //if (userDictionary.TryGetValue(request.Id, out IUser filteredUser))
            //{
            //    return Response<IUser>.GetSuccessResponse(filteredUser);
            //}
            //else
            //{
            //    return Response<IUser>.GetFailedResponse(new List<string> { MessageConstants.NoUserByThatId });
            //}
        }
    }
}