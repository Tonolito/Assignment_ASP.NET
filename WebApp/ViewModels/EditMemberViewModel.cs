using Domain.Dtos;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class EditMemberViewModel
{
    public string Id { get; set; } = null!;

    [DataType(DataType.Upload)]
    [Display(Name = "Member Image", Prompt = "Select a image")]
    public IFormFile? MemberImage { get; set; }

    [Display(Name = "First Name", Prompt = "Enter your First Name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Last Name", Prompt = "Enter your Last Name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Email", Prompt = "Enter email address")]
    //[RegularExpression("sadas")]
    [Required(ErrorMessage = "Required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Display(Name = "Phone", Prompt = "Enter Phone")]
    [DataType(DataType.PhoneNumber)]
    public string? Phone { get; set; }

    [Display(Name = "Job Title", Prompt = "Enter your Job Title")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string JobTitle { get; set; } = null!;

    [Display(Name = "Address", Prompt = "Enter your Address")]
    [DataType(DataType.Text)]
    public string? Address { get; set; }


    //public DateOnly? DateOfBirth { get; set; }

    public static implicit operator EditMemberDto(EditMemberViewModel model)
    {
        return model == null
            ? null!
            : new EditMemberDto
            {
                MemberImage = model.MemberImage,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                JobTitle = model.JobTitle,
                Address = model.Address,
                //DateOfBirth = model.DateOfBirth,

            };
    }
}
