namespace JobInja.Api.Entities;

public class Resume : BaseEntity
{
    public string Title { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string FilePath { get; set; } = null!;

    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public List<JobApplication> Applications { get; set; } = [];
}