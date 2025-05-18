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
using Business.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;


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
    public async Task<IActionResult> Add(AddProjectViewModel model, List<string> SelectedMemberIds, string SelectedClientId)
    {
        ViewBag.ErrorMessage = null;

        AddProjectDto dto = model;


        var SelectedM = SelectedMemberIds;

        var SelectedC = SelectedClientId;

        dto.SelectedMemberIds = SelectedM;
        dto.SelectedClientId = SelectedC;



        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(kvp => kvp.Key,
                kvp => kvp.Value?.Errors.Select(x => x.ErrorMessage).ToArray());
            return BadRequest(new { success = false, errors });
        }

        var result = await _projectService.CreateProjectAsync(dto);
        if (result.Succeeded)
        {
          

            return Ok();
        }
        else
        {
            return BadRequest(new { success = false });
        }
    }


    [HttpPut]
    [Route("edit")]

    public async Task<IActionResult> Edit(EditProjectViewModel model)
    {

        // Couldn't get edit project to populate the selectedids or id for client
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

    [HttpGet]
    [Route("edit/{id}")]
    public async Task<IActionResult> Edit(string id)
    {


        var result = await _projectService.GetProjectByIdAsync(id);


        if (result.Succeeded && result.Result != null)
        {

            return Json(new
            {
                id = result.Result.Id,
                projectName = result.Result.ProjectName,
                description = result.Result.Description,
                memberIds = result.Result.MemberIds,
                budget = result.Result.Budget,
                startDate = result.Result.StartDate,
                endDate = result.Result.EndDate,
                statusId = result.Result.StatusId,
                clientId = result.Result.Client.Id,
                client = new Client
                {
                    Id = result.Result.Client.Id,
                    Image = result.Result.Client.Image,
                    ClientName = result.Result.Client.ClientName
                },
                members = result.Result.Members

            }); ;

        }

        return Json(new { success = false, error = "Project not found." });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(string id)
    {

        if (string.IsNullOrEmpty(id))
        {
            return BadRequest(new { success = false, message = "Project ID cannot be null or empty" });
        }

        var result = await _projectService.DeleteProjectAsync(id);
        if (result.Succeeded)
        {
            return Ok(new { success = true, message = "Project deleted successfully." });
        }
        if (!result.Succeeded)
        {
            Debug.WriteLine($"Failed to delete project: {result.Error}");
        }

        return BadRequest(new { success = false, message = result.Error });
    }
}
