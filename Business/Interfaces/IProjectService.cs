﻿using Business.Models;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IProjectService
    {
        //Task<ProjectResult<string>> CreateProjectAsync(AddProjectDto dto, List<string> selectedMemberIds);
        Task<ProjectResult> CreateProjectAsync(AddProjectDto dto);
        Task<ProjectResult> DeleteProjectAsync(string id);
        Task<ProjectResult> GetProjectByIdAsync(string id);
        Task<ProjectsResult<IEnumerable<Project>>> GetProjectsAsync();
        //Task<ProjectResult<Project>> GetProjectsAsync(string id);
        Task<ProjectResult> UpdateProjectAsync(EditProjectDto dto);
        Task<ProjectResult> UpdateProjectMembersAsync(string projectId, string selectedMemberIdsJson);
    }
}