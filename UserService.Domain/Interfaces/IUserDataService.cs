using UserService.Domain.Models;
using UserService.Domain.Responses;

namespace UserService.Domain.Interfaces
{
    public interface IUserDataService
    {
        public Task<IResponse<IEnumerable<IUser>>> Get(CancellationToken cancellationToken);
    }
}