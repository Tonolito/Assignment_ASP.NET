using Domain.Dtos;
using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class EditProjectViewModel
{
    public string Id { get; set; } = null!;

    [DataType(DataType.Upload)]
    [Display(Name = "Project Image", Prompt = "Select a image")]
    public IFormFile? ProjectImage { get; set; }


    [Display(Name = "Project Name", Prompt = "Enter Project name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ProjectName { get; set; } = null!;

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

    public string? SelectedClientId { get; set; }



    public static implicit operator EditProjectDto(EditProjectViewModel model)
    {
        return model == null
            ? null!
            : new EditProjectDto
            {
                Id = model.Id,
                ProjectImage = model.ProjectImage,
                ProjectName = model.ProjectName,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Budget = model.Budget,
                SelectedMemberIds = model.SelectedMemberIds,
                SelectedClientId = model.SelectedClientId

            };
    }
}
