using Domain.Dtos;
using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels;

public class AddProjectViewModel
{
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
    public DateTime StartDate { get; set; }

    [Display(Name = "End Date", Prompt = "")]
    [DataType(DataType.Date)]
    [Required(ErrorMessage = "Required")]
    public DateTime EndDate { get; set; }

    [Display(Name = "Budget", Prompt = "0")]
    [Required(ErrorMessage = "Required")]
    public int Budget { get; set; }

    //??????
    [Display(Name = "Members", Prompt = "Add Member")]
    public string UserId { get; set; } = null!;

    [Display(Name = "Client Name", Prompt = "Enter Client name")]
    [DataType(DataType.Text)]
    [Required(ErrorMessage = "Required")]
    public string ClientId { get; set; } = null!;

    public static implicit operator AddProjectDto(AddProjectViewModel model)
    {
        return model == null
            ? null!
            : new AddProjectDto
            {
                ProjectImage = model.ProjectImage,
                ProjectName = model.ProjectName,
                Description = model.Description,
                StartDate = model.StartDate,
                EndDate = model.EndDate,
                Budget = model.Budget,
                UserId = model.UserId,
                ClientId = model.ClientId,

            };
    }
}
