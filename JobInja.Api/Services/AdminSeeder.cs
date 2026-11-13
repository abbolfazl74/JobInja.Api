using JobInja.Api.Constants;
using JobInja.Api.Data;
using JobInja.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobInja.Api.Services;

public static class AdminSeeder
{
    public static async Task SeedAdminAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<ApplicationDbContext>();
        var configuration = services.GetRequiredService<IConfiguration>();
        var passwordHasher = services.GetRequiredService<IPasswordHasherService>();

        var username = configuration["Settings:Admin:Username"];
        var password = configuration["Settings:Admin:Password"];

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine(AppMessages.AdminNotConfigured);
            return;
        }

        var normalizedUsername = username.Trim().ToUpperInvariant();

        var adminExists = await context.Users
            .AnyAsync(x => x.Username == normalizedUsername);

        if (adminExists)
        {
            return;
        }

        var adminRole = await context.Roles
            .FirstAsync(x => x.Name == RoleNames.Admin);

        var admin = new User
        {
            FirstName = configuration["Settings:Admin:FirstName"] ?? "مدیر",
            LastName = configuration["Settings:Admin:LastName"] ?? "سیستم",
            Mobile = configuration["Settings:Admin:Mobile"] ?? "00000000000",
            Username = normalizedUsername,
            PasswordHash = passwordHasher.Hash(password),
            UserRoles =
            [
                new UserRole { RoleId = adminRole.Id }
            ]
        };

        context.Users.Add(admin);
        await context.SaveChangesAsync();
    }
}