using System.ComponentModel.DataAnnotations;

namespace JobInja.Api.DTOs.Resumes;

public class CreateResumeRequestDto
{
    [Required(ErrorMessage = "عنوان رزومه الزامی می باشد")]
    [MaxLength(200, ErrorMessage = "عنوان نمی تواند بیشتر از 200 کاراکتر باشد")]
    public string Title { get; set; } = null!;

    [Required(ErrorMessage = "فایل رزومه الزامی می باشد")]
    public IFormFile File { get; set; } = null!;
}