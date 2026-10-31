namespace Backend.Application.Interfaces.Services;

public interface IUserService
{
    Task<UserDTO?> GetByUserIdAsync(Guid id, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);
}
