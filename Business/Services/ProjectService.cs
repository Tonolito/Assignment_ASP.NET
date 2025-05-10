using Business.Interfaces;
using Business.Models;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Domain.Dtos;
using Domain.Extentions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text.Json;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository, IStatusService statusService, IProjectMemberRepository projectMemberRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IStatusService _statusService = statusService;
private readonly IProjectMemberRepository _projectMemberRepository = projectMemberRepository;
    //CREATE
    //, List<string> selectedUserIds
    public async Task<ProjectResult<string>> CreateProjectAsync(AddProjectDto dto)
    {
        if (dto == null)
        {
            return new ProjectResult<string> { Succeeded = false, StatusCode = 400, Error = "Not all required fields are input." };
        }

        var projectEntity = dto.MapTo<ProjectEntity>();
        var statusResult = await _statusService.GetStatusByIdAsync(1);
        var status = statusResult.Result;

        projectEntity.StatusId = status!.Id;

        var result = await _projectRepository.AddAsync(projectEntity);

        if (result.Succeeded)
        {
            // Now fetch the created project with its details
            var projectWithDetails = await _projectRepository.GetProjectWithDetailsAsync(projectEntity.Id);

            return new ProjectResult<string>
            {
                Succeeded = true,
                StatusCode = 201,
                Result = projectEntity.Id // Return the generated ProjectId
            };
        }
        else
        {
            return new ProjectResult<string>
            {
                Succeeded = false,
                StatusCode = result.StatusCode,
                Error = result.Error
            };
        }
    }

    //READ - Get all projects with members and clients
    public async Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync()
    {
        var response = await _projectRepository.GetAllAsync(orderByDescending: true, sortBy: s => s.Created, where: null, include => include.ProjectMembers, include => include.Status, include => include.ProjectClients);

        return new ProjectResult<IEnumerable<Project>> { Succeeded = true, StatusCode = 200, Result = response.Result };
    }

    //READ - Get a specific project with members and clients
    public async Task<ProjectResult<Project>> GetProjectsAsync(string id)
    {
        var projectEntity = await _projectRepository.GetProjectWithDetailsAsync(id);

        return projectEntity != null
            ? new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Result = projectEntity.MapTo<Project>() }
            : new ProjectResult<Project> { Succeeded = false, StatusCode = 404, Error = $"Project with '{id}' was not found" };
    }

    //UPDATE
    public async Task<ProjectResult> UpdateProjectAsync(EditProjectDto dto)
    {
        if (dto == null)
        {
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Invalid project data." };
        }

        var projectEntity = dto.MapTo<ProjectEntity>();
        var projectResult = await _projectRepository.GetAsync(x => x.Id == dto.Id);

        var result = await _projectRepository.UpdateAsync(projectEntity);

        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 200 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    //DELETE
    public async Task<ProjectResult> DeleteProjectAsync(string id)
    {
        if (id == null)
        {
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Invalid project ID." };
        }

        var projectResult = await _projectRepository.GetAsync(x => x.Id == id);
        var projectEntity = projectResult.Result!.MapTo<ProjectEntity>();

        var result = await _projectRepository.DeleteAsync(projectEntity);
        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 200 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
    }

    //MSC - Update project members (unchanged)
    public async Task<ProjectResult> UpdateProjectMembersAsync(string projectId, string selectedMemberIdsJson)
    {
        if (string.IsNullOrEmpty(projectId))
        {
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Project ID cannot be null or empty." };
        }

        if (string.IsNullOrWhiteSpace(selectedMemberIdsJson))
        {
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Member IDs are required." };
        }

        try
        {
            // Deserialize selected member IDs
            var memberIds = JsonSerializer.Deserialize<List<string>>(selectedMemberIdsJson);

            if (memberIds == null || !memberIds.Any())
            {
                return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "No valid member IDs provided." };
            }

            // Remove all members for the project first
            await _projectMemberRepository.RemoveAllByProjectIdAsync(projectId);

            // Add new members to the project
            await _projectMemberRepository.AddMembersAsync(projectId, memberIds);

            return new ProjectResult { Succeeded = true, StatusCode = 200 };
        }
        catch (JsonException ex)
        {
            // Specific exception for deserialization issues
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = $"Invalid member IDs format: {ex.Message}" };
        }
        catch (Exception ex)
        {
            // Catch any other unforeseen exceptions
            return new ProjectResult { Succeeded = false, StatusCode = 500, Error = $"An error occurred: {ex.Message}" };
        }
    }
}


