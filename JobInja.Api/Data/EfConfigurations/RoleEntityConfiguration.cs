
using JobInja.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobInja.Api.Data.EfConfigurations;

public class RoleEntityConfiguration : IEntityTypeConfiguration<Role>
{
    private static readonly DateTime SeedDate = new(2026, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.Name).IsUnique();

        builder.HasData(
     new Role { Id = 1, Name = "ADMIN", DisplayName = "مدیر سیستم", DateCreated = SeedDate },
     new Role { Id = 2, Name = "EMPLOYER", DisplayName = "کارفرما", DateCreated = SeedDate },
     new Role { Id = 3, Name = "JOBSEEKER", DisplayName = "کارجو", DateCreated = SeedDate });
    }
}