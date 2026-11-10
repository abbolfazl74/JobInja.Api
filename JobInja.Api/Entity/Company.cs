namespace JobInja.Api.Entities;

public class Company : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;

    public int OwnerId { get; set; }
    public int ProvinceId { get; set; }

    public User Owner { get; set; } = null!;
    public Province Province { get; set; } = null!;
    public List<JobPost> JobPosts { get; set; } = [];
}