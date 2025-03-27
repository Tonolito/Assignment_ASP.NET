using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.Dtos;

public class AddClientDto
{
    [DataType(DataType.Upload)]
    [Display(Name = "Client Image", Prompt = "Select a image")]
    public IFormFile? ClientImage { get; set; }


    [Display(Name = "Company Name", Prompt = "Enter company name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string CompanyName { get; set; } = null!;

    [Display(Name = "Email", Prompt = "Enter email address")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid Email")]
    [Required(ErrorMessage = "Required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Display(Name = "Location", Prompt = "Enter location")]
    [DataType(DataType.Text)]
    public string? Location { get; set; }


    [Display(Name = "Phone", Prompt = "Enter Phonenumber")]
    [DataType(DataType.PhoneNumber)]
    public string? Phone { get; set; }
}
