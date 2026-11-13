namespace JobInja.Api.DTOs.JobPosts;

public class JobPostResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? SalaryText { get; set; }
    public string CompanyName { get; set; } = null!;
    public string ProvinceName { get; set; } = null!;
    public DateTime DateCreated { get; set; }
}