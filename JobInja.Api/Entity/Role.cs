namespace JobInja.Api.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; } = null!;

    public List<UserRole> UserRoles { get; set; } = [];
}