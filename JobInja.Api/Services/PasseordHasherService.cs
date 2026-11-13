using Microsoft.AspNetCore.Identity;

namespace JobInja.Api.Services;

public class PasswordHasherService : IPasswordHasherService
{
    private readonly PasswordHasher<object> _passwordHasher = new();

    public string Hash(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentNullException(nameof(password));

        return _passwordHasher.HashPassword(null!, password);
    }

    public bool Verify(string passwordHash, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(null!, passwordHash, password);
        return result == PasswordVerificationResult.Success;
    }
}