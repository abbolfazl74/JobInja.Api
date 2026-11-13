using System.ComponentModel.DataAnnotations;

namespace JobInja.Api.DTOs.Auth;

public class LoginRequestDto
{
    [Required(ErrorMessage = "نام کاربری الزامی می باشد")]
    public string Username { get; set; } = null!;

    [Required(ErrorMessage = "کلمه عبور الزامی می باشد")]
    public string Password { get; set; } = null!;
}