using UserService.Domain.Models;

namespace UserService.Domain.Interfaces
{
    public interface IUserDataService
    {
        public Task<IEnumerable<IUser>> Get(CancellationToken cancellationToken);
    }
}