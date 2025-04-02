using Domain.Dtos;
using Domain.Models;

namespace WebApp.ViewModels;

public class ProjectsViewModel
{
    public IEnumerable<Project> Projects { get; set; } = [];

    public AddProjectViewModel AddProject { get; set; } = null!;


    public EditProjectViewModel Edit { get; set; } = null!;

 
}
