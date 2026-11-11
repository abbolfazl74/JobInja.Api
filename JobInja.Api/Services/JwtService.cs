using JobInja.Api.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobInja.Api.Services;

public class JwtService(IConfiguration configuration) : IJwtService
{
    public string GenerateToken(User user)
    {
        var jwtSecret = configuration.GetSection("Settings:JwtSecret").Value!;
        var issuer = configuration.GetSection("Settings:Issuer").Value!;
        var audience = configuration.GetSection("Settings:Audience").Value!;

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new (ClaimTypes.NameIdentifier,user.Id.ToString())
        };


        foreach (var userRole in user.UserRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
        }


        var token = new JwtSecurityToken
        (
            issuer: issuer,
            audience: audience,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddHours(4),
            signingCredentials: credentials,
            claims: claims

        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;

    }
}