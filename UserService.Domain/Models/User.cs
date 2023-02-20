using System.Text.Json.Serialization;

namespace UserService.Domain.Models
{
    public interface IUser
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("first")]
        public string First { get; set; }

        [JsonPropertyName("last")]
        public string Last { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }
    }

    public class User : IUser
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("first")]
        public string First { get; set; }

        [JsonPropertyName("last")]
        public string Last { get; set; }

        [JsonPropertyName("age")]
        public int Age { get; set; }

        [JsonPropertyName("gender")]
        public string Gender { get; set; }
    }
}