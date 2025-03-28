using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace Domain.Dtos;

public class AddProjectDto
{
    [DataType(DataType.Upload)]
    [Display(Name = "Project Image", Prompt = "Select a image")]
    public IFormFile? ProjectImage { get; set; }


    [Display(Name = "Project Name", Prompt = "Enter Project name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ProjectName { get; set; } = null!;

    [Display(Name = "Client Name", Prompt = "Enter Client name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ClientName { get; set; } = null!;

    [Display(Name = "Description", Prompt = "Enter Description")]
    [Required(ErrorMessage = "Required")]
    [DataType(DataType.Text)]
    public string Description { get; set; } = null!;

    [Display(Name = "Start Date", Prompt = "")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Required")]
    public DateOnly StartDate { get; set; }

    [Display(Name = "End Date", Prompt = "")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Required")]
    public DateOnly EndDate { get; set; }


    //??????
    [Display(Name = "Members", Prompt = "Add Member")]
    public string? Members { get; set; }

    [Display(Name = "Budget", Prompt = "0")]
    [Required(ErrorMessage = "Required")]
    public int Budget { get; set; }
}
