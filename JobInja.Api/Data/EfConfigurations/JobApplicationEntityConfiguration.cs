
using JobInja.Api.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JobInja.Api.Data.EfConfigurations;

public class JobApplicationEntityConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        builder.ToTable("JobApplications");

        builder.HasOne(x => x.JobPost)
            .WithMany(x => x.Applications)
            .HasForeignKey(x => x.JobPostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.User)
            .WithMany(x => x.JobApplications)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Resume)
            .WithMany(x => x.Applications)
            .HasForeignKey(x => x.ResumeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasIndex(x => new { x.JobPostId, x.UserId }).IsUnique();
    }
}