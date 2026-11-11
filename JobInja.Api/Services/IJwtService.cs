using JobInja.Api.Entities;

namespace JobInja.Api.Services
{
    public interface IJwtService
    {
        string GenerateToken(User user);
    }
}