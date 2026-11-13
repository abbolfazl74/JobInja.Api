using JobInja.Api.Constants;
using JobInja.Api.Data;
using JobInja.Api.DTOs.Auth;
using JobInja.Api.Entities;
using JobInja.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JobInja.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(
    ApplicationDbContext context,
    IPasswordHasherService passwordHasherService,
    IJwtService jwtService) : ControllerBase
{
    [HttpPost("register/jobseeker")]
    public async Task<ActionResult> RegisterJobSeeker(RegisterRequestDto request)
    {
        return await RegisterWithRole(request, RoleNames.JobSeeker);
    }

    [HttpPost("register/employer")]
    public async Task<ActionResult> RegisterEmployer(RegisterRequestDto request)
    {
        return await RegisterWithRole(request, RoleNames.Employer);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginRequestDto request)
    {
        var normalizedUsername = request.Username.Trim().ToUpperInvariant();

        var user = await context.Users
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .FirstOrDefaultAsync(x => x.Username == normalizedUsername);

        if (user is null)
        {
            return BadRequest(AppMessages.InvalidCredentials);
        }

        var isPasswordCorrect = passwordHasherService
            .Verify(user.PasswordHash, request.Password);

        if (!isPasswordCorrect)
        {
            return BadRequest(AppMessages.InvalidCredentials);
        }

        var token = jwtService.GenerateToken(user);

        return Ok(new { Token = token });
    }

    private async Task<ActionResult> RegisterWithRole(RegisterRequestDto request, string roleName)
    {
        var normalizedUsername = request.Username.Trim().ToUpperInvariant();

        if (await context.Users.AnyAsync(x => x.Username == normalizedUsername))
        {
            return BadRequest(AppMessages.UsernameAlreadyExists);
        }

        var role = await context.Roles
            .FirstOrDefaultAsync(x => x.Name == roleName);

        if (role is null)
        {
            return BadRequest(AppMessages.NotFound);
        }

        var user = new User
        {
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Mobile = request.Mobile,
            Username = normalizedUsername,
            PasswordHash = passwordHasherService.Hash(request.Password),
            UserRoles =
            [
                new UserRole { RoleId = role.Id }
            ]
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return Created($"api/users/{user.Id}", new { Id = user.Id });
    }
}