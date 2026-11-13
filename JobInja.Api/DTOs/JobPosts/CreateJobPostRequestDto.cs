using System.ComponentModel.DataAnnotations;

namespace JobInja.Api.DTOs.JobPosts;

public class CreateJobPostRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = "شرکت را انتخاب کنید")]
    public int CompanyId { get; set; }

    [Required(ErrorMessage = "عنوان آگهی الزامی می باشد")]
    [MaxLength(200, ErrorMessage = "عنوان نمی تواند بیشتر از 200 کاراکتر باشد")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "شرح آگهی الزامی می باشد")]
    [MaxLength(4000, ErrorMessage = "شرح نمی تواند بیشتر از 4000 کاراکتر باشد")]
    public string Description { get; set; } = null!;

    [MaxLength(100, ErrorMessage = "متن حقوق نمی تواند بیشتر از 100 کاراکتر باشد")]
    public string? SalaryText { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "استان را انتخاب کنید")]
    public int ProvinceId { get; set; }
}