using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class EditClientForm
{
    //public int Id { get; set; }


    [DataType(DataType.Upload)]
    [Display(Name = "Client Image", Prompt = "Select a image")]
    public IFormFile? ClientImage { get; set; }


    [Display(Name = "Company Name", Prompt = "Enter company name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string CompanyName { get; set; } = null!;

    [Display(Name = "Email", Prompt = "Enter email address")]
    //[RegularExpression("sadas")]
    [Required(ErrorMessage = "Required")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;

    [Display(Name = "Location", Prompt = "Enter location")]
    [DataType(DataType.Text)]
    public string? Location { get; set; }


    [Display(Name = "Phone", Prompt = "Enter     Phonenumber")]
    [DataType(DataType.PhoneNumber)]
    public string? Phone { get; set; }
}
