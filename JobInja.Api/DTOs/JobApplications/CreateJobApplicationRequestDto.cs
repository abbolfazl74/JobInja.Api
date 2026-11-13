using System.ComponentModel.DataAnnotations;

namespace JobInja.Api.DTOs.JobApplications;

public class CreateJobApplicationRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = "آگهی را انتخاب کنید")]
    public int JobPostId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "رزومه را انتخاب کنید")]
    public int ResumeId { get; set; }
}