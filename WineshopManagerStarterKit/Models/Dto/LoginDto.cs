using System.ComponentModel.DataAnnotations;

namespace WineshopManagerStarterKit.Models.Dto;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}