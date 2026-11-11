using JobInja.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobInja.Api.Data;

public class ApplicationDbContext : DbContext
{

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<Province> Provinces { get; set; } = null!;
    public DbSet<Company> Companies { get; set; } = null!;
    public DbSet<JobPost> JobPosts { get; set; } = null!;
    public DbSet<Resume> Resumes { get; set; } = null!;
    public DbSet<JobApplication> JobApplications { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
