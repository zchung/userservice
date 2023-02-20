using System.Text.Json;
using UserService.Domain.Models;
using UserService.Domain.Responses;

namespace UserService.Extensions
{
    public static class ResponseExtensions
    {
        public static void WriteToConsole(this IResponse<string> response)
        {
            if (response.Success)
            {
                Console.WriteLine(response.Data);
            }
            else
            {
                response.Messages.ForEach(x => Console.WriteLine(x));
            }
        }

        public static void WriteToConsole(this IResponse<IEnumerable<IUser>> response)
        {
            if (response.Success)
            {
                Console.WriteLine(JsonSerializer.Serialize(response.Data, new JsonSerializerOptions { WriteIndented = true }));
            }
            else
            {
                response.Messages.ForEach(x => Console.WriteLine(x));
            }
        }
    }
}