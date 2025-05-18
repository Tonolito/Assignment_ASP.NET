using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class EditProjectDto
{
    public string? Id { get; set; }

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
    public DateTime? StartDate { get; set; }

    [Display(Name = "End Date", Prompt = "")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Required")]
    public DateTime? EndDate { get; set; }

    [Display(Name = "Budget", Prompt = "0")]
    [Required(ErrorMessage = "Required")]
    public decimal? Budget { get; set; }

    public List<string> SelectedMemberIds { get; set; } = new();

    public string SelectedClientId { get; set; } = null!;


}
