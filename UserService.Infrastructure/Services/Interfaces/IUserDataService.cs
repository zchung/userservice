using UserService.Domain.Models;

namespace UserService.Infrastructure.Services.Interfaces
{
    public interface IUserDataService
    {
        public Task<IEnumerable<IUser>> Get(CancellationToken cancellationToken);
    }
}