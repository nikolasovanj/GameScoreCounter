using Backend.Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Backend.Infrastructure.Security;

public class PasswordService(IOptions<PasswordHasherOptions> options) : IPasswordService
{
    private readonly PasswordHasher<object> _hasher = new(options);
    public string Hash(string password) => _hasher.HashPassword(null!, password);

    public bool Verify(string hash, string password, out bool needsRehash)
    {
        var result = _hasher.VerifyHashedPassword(null!, hash, password);
        needsRehash = result == PasswordVerificationResult.SuccessRehashNeeded;
        return result != PasswordVerificationResult.Failed;
    }
}