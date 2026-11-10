namespace JobInja.Api.Entities;

public class JobApplication : BaseEntity
{
    public int JobPostId { get; set; }
    public int UserId { get; set; }
    public int ResumeId { get; set; }
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Submitted;

    public JobPost JobPost { get; set; } = null!;
    public User User { get; set; } = null!;
    public Resume Resume { get; set; } = null!;
}