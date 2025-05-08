using Domain.Dtos;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class EditClientViewModel
{
    public string Id { get; set; } = null!;


    [DataType(DataType.Upload)]
    [Display(Name = "Client Image", Prompt = "Select a image")]
    public IFormFile? ClientImage { get; set; }


    [Display(Name = "Client Name", Prompt = "Enter company name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ClientName { get; set; } = null!;

    [Display(Name = "Email", Prompt = "Enter email address")]
    //[RegularExpression("sadas")]
    [Required(ErrorMessage = "Required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Display(Name = "Location", Prompt = "Enter location")]
    [DataType(DataType.Text)]
    public string? Location { get; set; }


    [Display(Name = "Phone", Prompt = "Enter Phone")]
    [DataType(DataType.PhoneNumber)]
    public string? Phone { get; set; }

    public static implicit operator EditClientDto(EditClientViewModel model)
    {
        return model == null
            ? null!
            : new EditClientDto
            {
                Id = model.Id,
                ClientImage = model.ClientImage,
                ClientName = model.ClientName,
                Email = model.Email,
                Location = model.Location,
                Phone = model.Phone,
            };
    }
}
