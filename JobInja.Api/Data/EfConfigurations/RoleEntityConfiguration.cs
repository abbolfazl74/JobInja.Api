using JobInja.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobInja.Api.Data.EfConfigurations;

public class RoleEntityConfiguration
 : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");

        builder.Property(x => x.Name)
           .IsRequired()
           .HasMaxLength(100);

        builder.Property(x => x.DisplayName)
          .IsRequired()
          .HasMaxLength(100);



    }

}
