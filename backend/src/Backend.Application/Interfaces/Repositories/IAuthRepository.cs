using Backend.Application.DTOs.Requests;
using Backend.Domain.Entities;

namespace Backend.Application.Interfaces.Repositories;

public interface IAuthRepository
{
    Task<User?> Register(User user, CancellationToken ct = default);
    Task<User?> GetExisting(string username, CancellationToken ct = default);
}
