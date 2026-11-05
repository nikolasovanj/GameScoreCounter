using Backend.Domain.Result;

namespace Backend.Application.Interfaces.Services;

public interface IUserService
{
    Task<Result<UserDTO?>> GetByUserIdAsync(Guid id, CancellationToken ct = default);
    Task<Result<UserDTO?>> GetByUsenameAsync(string username, CancellationToken ct = default);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct = default);
}
