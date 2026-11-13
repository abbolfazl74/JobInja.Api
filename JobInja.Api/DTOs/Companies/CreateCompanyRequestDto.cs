using System.ComponentModel.DataAnnotations;

namespace JobInja.Api.DTOs.Companies;

public class CreateCompanyRequestDto
{
    [Required(ErrorMessage = "نام شرکت الزامی می باشد")]
    [MaxLength(200, ErrorMessage = "نام شرکت نمی تواند بیشتر از 200 کاراکتر باشد")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "توضیحات شرکت الزامی می باشد")]
    [MaxLength(2000, ErrorMessage = "توضیحات نمی تواند بیشتر از 2000 کاراکتر باشد")]
    public string Description { get; set; } = null!;

    [Range(1, int.MaxValue, ErrorMessage = "استان را انتخاب کنید")]
    public int ProvinceId { get; set; }
}