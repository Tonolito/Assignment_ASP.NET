using Business.Interfaces;
using Data.Contexts;
using Data.Entities;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components.Forms;


namespace WebApp.Controllers;

[Authorize]
[Route("projects")]
public class ProjectsController(IProjectService projectService, AppDbContext dataContext) : Controller
{

    private readonly IProjectService _projectService = projectService;
    private readonly AppDbContext _context = dataContext;

    [Route("")]
    public async Task<IActionResult> Projects()
    {
        //Får inte med status eller selectedMemberIDs
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
    [Route("projects/Add")]
    public async Task<IActionResult> Add(AddProjectViewModel model, List<string> SelectedMemberIds)
    {
        ViewBag.ErrorMessage = null;

        AddProjectDto dto = model;


        var SelectedM = SelectedMemberIds;

        dto.SelectedMemberIds = SelectedM;

        // Convert view model to DTO    

        // Check for model validation
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray());
            return BadRequest(new { success = false, errors });
        }

        // Create project in the service
        var result = await _projectService.CreateProjectAsync(dto);
        if (result.Succeeded)
        {
            // Update members using the ProjectId that was returned in Result
            //await _projectService.UpdateProjectMembersAsync(result.Result, SelectedMemberIds);

            // Return the success response with the ProjectId in the result
            return Ok();
        }
        else
        {
            return BadRequest(new { success = false });
        }
    }


    [HttpPost]
    [Route("projects/edit")]

    public async Task<IActionResult> Edit(EditProjectViewModel model)
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
