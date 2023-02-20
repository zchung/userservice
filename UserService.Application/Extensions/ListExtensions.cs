namespace UserService.Application.Extensions
{
    public static class ListExtensions
    {
        public static string ToJoinedString(this IEnumerable<string> list, string? separator = ",")
        {
            return string.Join(separator, list);
        }
    }
}