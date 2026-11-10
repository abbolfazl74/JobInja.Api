namespace JobInja.Api.Entities;

public class JobPost : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string? SalaryText { get; set; }
    public bool IsActive { get; set; } = true;

    public int CompanyId { get; set; }
    public int ProvinceId { get; set; }

    public Company Company { get; set; } = null!;
    public Province Province { get; set; } = null!;
    public List<JobApplication> Applications { get; set; } = [];
}