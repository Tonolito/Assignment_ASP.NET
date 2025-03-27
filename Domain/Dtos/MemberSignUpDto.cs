using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class MemberSignUpDto
{
    [Required]
    [Display(Name = "First Name", Prompt = "Enter Firstname")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Display(Name = "Last Name", Prompt = "Enter Lastname")]
    public string LastName { get; set; } = null!;

    [Required]
    [Display(Name = "Email", Prompt = "Enter Email")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required]
    [Display(Name = "Password", Prompt = "Enter Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Compare(nameof(Password), ErrorMessage ="Password don't match")]
    [Display(Name = "Confrim Password", Prompt = "Confirm your Password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;
}
