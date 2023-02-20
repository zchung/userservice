using MediatR;
using UserService.Application.Handlers.Queries;
using UserService.Domain.Models;
using UserService.Domain.Responses;
using UserService.Extensions;

// Worker.cs
internal class UserWorker
{
    private readonly IMediator _mediator;

    public UserWorker(IMediator mediator)
    {
        _mediator = mediator;
    }

    public void GetAllUsers()
    {
        Task<IResponse<IEnumerable<IUser>>> response = Task.Run(() => _mediator.Send(new GetUsersQuery()));
        response.Result.WriteToConsole();
    }

    public void GetUserFullNameById(int id)
    {
        Task<IResponse<IUser>> response = Task.Run(() => _mediator.Send(new GetUserByIdQuery(id)));
        var result = response.Result;
        if (result.Success)
        {
            Console.WriteLine(result.Data.FullName);
        }
        else
        {
            foreach (var message in result.Messages)
            {
                Console.WriteLine(message);
            }
        }
    }
}