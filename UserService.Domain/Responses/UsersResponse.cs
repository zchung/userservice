using UserService.Domain.Models;

namespace UserService.Domain.Responses
{
    public interface IUsersResponse
    {
        public IEnumerable<IUser> Users { get; }
    }

    public class UsersResponse : IUsersResponse
    {
        public IEnumerable<IUser> Users { get; }

        public UsersResponse(IEnumerable<IUser> users)
        {
            Users = users;
        }
    }
}