using JobInja.Api.Entities;

namespace JobInja.Api.DTOs.JobApplications;

public class ApplicantResponseDto
{
    public int Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Mobile { get; set; } = null!;
    public int ResumeId { get; set; }
    public string ResumeTitle { get; set; } = null!;
    public ApplicationStatus Status { get; set; }
    public DateTime DateCreated { get; set; }
}