namespace JobInja.Api.DTOs.JobPosts;

public class JobPostSearchRequestDto
{
    public string? Keyword { get; set; }

    public int? ProvinceId { get; set; }
}