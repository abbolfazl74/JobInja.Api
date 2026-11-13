using JobInja.Api.Entities;

namespace JobInja.Api.DTOs.JobApplications;

public class JobApplicationResponseDto
{
    public int Id { get; set; }
    public int JobPostId { get; set; }
    public string JobPostTitle { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public ApplicationStatus Status { get; set; }
    public DateTime DateCreated { get; set; }
}