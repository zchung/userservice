namespace UserService.Domain.Responses
{
    public interface IResponse<T>
    {
        public bool Success { get; set; }
        public List<string> Messages { get; set; }
        public T Data { get; set; }
    }

    public class Response<T> : IResponse<T>
    {
        public bool Success { get; set; }
        public List<string> Messages { get; set; }
        public T Data { get; set; }

        public Response(bool success, T data, List<string> messages = null)
        {
            Success = success;
            Data = data;
            Messages = messages ?? new List<string>();
        }

        public static Response<T> GetSuccessResponse(T data)
        {
            return new Response<T>(true, data);
        }

        public static Response<T> GetFailedResponse(List<string> messages)
        {
            return new Response<T>(false, default, messages);
        }
    }
}