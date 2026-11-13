namespace JobInja.Api.DTOs.Resumes;

public class ResumeResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public DateTime DateCreated { get; set; }
}