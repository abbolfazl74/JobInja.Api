namespace JobInja.Api.Entities;

public class Province : BaseEntity
{
    public string Name { get; set; } = null!;

    public List<Company> Companies { get; set; } = [];
    public List<JobPost> JobPosts { get; set; } = [];
}