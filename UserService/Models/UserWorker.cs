using MediatR;
using UserService.Application.Extensions;
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
        Console.WriteLine($"Result for id: {id} - {nameof(GetUserFullNameById)}:");
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

    public void GetUsersByAge(int age)
    {
        Task<IResponse<IEnumerable<IUser>>> response = Task.Run(() => _mediator.Send(new GetUsersByAgeQuery(age)));
        var result = response.Result;
        Console.WriteLine($"Result for age: {age} - {nameof(GetUsersByAge)}:");
        if (result.Success)
        {
            Console.WriteLine(result.Data.Select(s => s.FullName).ToJoinedString());
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