using Business.Interfaces;
using Business.Models;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Dtos;
using Domain.Extentions;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository, IStatusService statusService, AppDbContext context) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IStatusService _statusService = statusService;
    private readonly AppDbContext _context = context;

    //CREATE
    //, List<string> selectedUserIds
    public async Task<ProjectResult> CreateProjectAsync(AddProjectDto dto) 
    {
        if (dto == null)
        {
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Not all requried fields are input." };
        }

        var projectEntity = dto.MapTo<ProjectEntity>();
        var statusResult = await _statusService.GetStatusByIdAsync(1);
        var status = statusResult.Result;

        projectEntity.StatusId = status!.Id;

        var result = await _projectRepository.AddAsync(projectEntity);

        //if (selectedUserIds != null && selectedUserIds.Any())
        //{
        //    foreach (var userId in selectedUserIds)
        //    {
        //        var projectMember = new ProjectMemberEntity
        //        {
        //            ProjectId = projectEntity.Id,
        //            MemberId = userId
        //        };
        //        await _context.AddAsync(projectMember);  // Lägg till koppling mellan projekt och medlem
        //    }
        //}

        //// Spara alla ändringar i databasen
        //await _context.SaveChangesAsync();

        return result.Succeeded
            ? new ProjectResult { Succeeded = true, StatusCode = 201 }
            : new ProjectResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };




    }

    //READ
    public async Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync()
    {
        var response = await _projectRepository.GetAllAsync(orderByDescending: true, sortBy: s => s.Created, where: null, include => include.Member, include => include.Status, include => include.Client);

        //return response.MapTo<ProjectResult<IEnumerable<Project>>>();

        return new ProjectResult<IEnumerable<Project>> { Succeeded = true, StatusCode = 200, Result = response.Result };
    }

    public async Task<ProjectResult<Project>> GetProjectsAsync(string id)
    {
        var response = await _projectRepository.GetAsync(where: x => x.Id == id, include => include.Member, include => include.Status, include => include.Client);

        return response.Succeeded
            ? new ProjectResult<Project> { Succeeded = true, StatusCode = 200, Result = response.Result }
            : new ProjectResult<Project> { Succeeded = false, StatusCode = 404, Error = $"Project with '{id} was not found" };
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

}
