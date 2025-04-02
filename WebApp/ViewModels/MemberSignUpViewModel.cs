using Domain.Dtos;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class MemberSignUpViewModel
{
    [Required]
    [Display(Name = "First Name", Prompt = "Enter Firstname")]
    public string FirstName { get; set; } = null!;

    [Required]
    [Display(Name = "Last Name", Prompt = "Enter Lastname")]
    public string LastName { get; set; } = null!;

    [Required]
    [Display(Name = "Email", Prompt = "Enter Email")]
    //[RegularExpression(@"")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Required]
    [Display(Name = "Password", Prompt = "Enter Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Compare(nameof(Password), ErrorMessage = "Password don't match")]
    [Display(Name = "Confrim Password", Prompt = "Confirm your Password")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; } = null!;

    // FUNGERAR EJ
    //[Range(typeof(bool),"true", "true")]
    //public bool TermsAndConditions { get; set; }


    public static implicit operator MemberSignUpDto(MemberSignUpViewModel model)
    {
        return model == null
            ? null!
            : new MemberSignUpDto
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
            };
    }
}
