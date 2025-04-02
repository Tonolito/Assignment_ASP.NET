using Domain.Dtos;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class MemberSignInViewModel
{
    [Required]
    [Display(Name = "Email", Prompt = "Enter Email")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
    [Required]
    [Display(Name = "Password", Prompt = "Enter Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public static implicit operator MemberSignInDto(MemberSignInViewModel model)
    {
        return model == null
            ? null!
            : new MemberSignInDto
            {
                Email = model.Email,
                Password = model.Password,  
            };
    }
}
