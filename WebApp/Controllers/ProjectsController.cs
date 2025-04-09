using Business.Interfaces;
using Data.Contexts;
using Data.Entities;
using Domain.Dtos;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WebApp.ViewModels;

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

    public async Task<IActionResult> Add(AddProjectViewModel model, string selectedUserIds)
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


        //var existingMembers = await _context.Users
        //    .Where(m => m.Id == model.id)
        //    .ToListAsync();

        //_context.Users.RemoveRange(existingMembers);

        //if (!string.IsNullOrEmpty(selectedUserIds))
        //{
        //    var userIds = JsonSerializer.Deserialize<List<int>>(selectedUserIds);
        //    if (userIds != null)
        //    {
        //        var projectMember = new ProjectMemberEntity
        //        {
        //            ProjectId = projectEntity.Id,
        //            MemberId = userId
        //        };
        //    }
        //}
        //_context.Update(model);
        //await _context.SaveChangesAsync();

        var result = await _projectService.CreateProjectAsync(model);
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
