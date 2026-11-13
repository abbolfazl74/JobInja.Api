namespace JobInja.Api.DTOs.Companies;

public class CompanyResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int ProvinceId { get; set; }
    public string ProvinceName { get; set; } = null!;
}