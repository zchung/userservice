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

    public void WriteUserFullNameById(int id)
    {
        Task<IResponse<IUser>> response = Task.Run(() => _mediator.Send(new GetUserByIdQuery(id)));
        var result = response.Result;
        Console.WriteLine($"Result for id: {id} - {nameof(WriteUserFullNameById)}:");
        if (result.Success)
        {
            Console.WriteLine(result.Data.FullName);
        }
        else
        {
            WriteMessagesToConsole(result.Messages);
        }
    }

    public void WriteUsersByAge(int age)
    {
        Task<IResponse<IEnumerable<IUser>>> response = Task.Run(() => _mediator.Send(new GetUsersByAgeQuery(age)));
        var result = response.Result;
        Console.WriteLine($"Result for age: {age} - {nameof(WriteUsersByAge)}:");
        if (result.Success)
        {
            Console.WriteLine(result.Data.Select(s => s.FullName).ToJoinedString());
        }
        else
        {
            WriteMessagesToConsole(result.Messages);
        }
    }

    public void WriteUserGenderNumbersByAge()
    {
        Task<IResponse<IEnumerable<IUserGenderNumberData>>> response = Task.Run(async () => await _mediator.Send(new GetUserGenderNumbersByAgeQuery()));
        var result = response.Result;
        Console.WriteLine($"Result for {nameof(WriteUserGenderNumbersByAge)}:");
        if (result.Success)
        {
            foreach (var data in result.Data)
            {
                Console.WriteLine(data.ToString());
            }
        }
        else
        {
            WriteMessagesToConsole(result.Messages);
        }
    }

    private void WriteMessagesToConsole(List<string> messages)
    {
        foreach (var message in messages)
        {
            Console.WriteLine(message);
        }
    }
}