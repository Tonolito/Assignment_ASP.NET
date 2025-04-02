using Business.Interfaces;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.ViewModels;

namespace WebApp.Controllers;

[Authorize]
[Route("projects")]
public class ProjectsController(IProjectService projectService) : Controller
{

    private readonly IProjectService _projectService = projectService;

    [Route("")]
    public async Task<IActionResult> Projects()
    {
        var projects = await _projectService.GetProjectsAsync();

        var viewModel = new ProjectsViewModel()
        {
            Projects = projects.Result ?? [],
            AddProject = new(),
            Edit = new()

        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> AddProject(AddProjectViewModel model)
    {
        ViewBag.ErrorMessage = null;

        AddProjectDto dto = model;

        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );
            return BadRequest(new { success = false, errors });
        }

        var result = await  _projectService.CreateProjectAsync(model);
        if (result.Succeeded)
        {
            return Ok(new { success = true });

        }
        else
        {
            return BadRequest(new { success = false });
        }

    }
    [HttpPost]
    public async Task<IActionResult> EditProject(EditProjectViewModel model)
    {
        EditProjectDto dto = model;

        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray()
                );
            return BadRequest(new { success = false, errors });
        }

        var result = await _projectService.UpdateProjectAsync(model);
        if (result.Succeeded)
        {
            return Ok(new { success = true });

        }
        return Ok(new { succes = true });

    }
}
