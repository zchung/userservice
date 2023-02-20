using UserService.Domain.Models;

namespace UserService.Application.Tests.Handlers.Queries.Helpers
{
    public class TestDataHelper
    {
        public static IEnumerable<IUser> GetTestUsers()
        {
            return new List<User>()
            {
                // fixed data from sample for testing purposes
                new User { Id = 53, First = "Bill", Last = "Bryson", Age = 23, Gender = "M" },
                new User { Id = 62, First = "John", Last = "Travolta", Age = 54, Gender = "M" },
                new User { Id = 41, First = "Frank", Last = "Zappa", Age = 23, Gender = "F" },
                new User { Id = 31, First = "Jill", Last = "Scott", Age = 66, Gender = "M" },
                new User { Id = 31, First = "Anna", Last = "Meredith", Age = 66, Gender = "M" },
                new User { Id = 31, First = "Janet", Last = "Jackson", Age = 66, Gender = "F" }
            };
        }
    }
}