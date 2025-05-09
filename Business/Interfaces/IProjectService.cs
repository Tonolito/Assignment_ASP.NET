using Business.Models;
using Domain.Dtos;
using Domain.Models;

namespace Business.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectResult> CreateProjectAsync(AddProjectDto dto);
        Task<ProjectResult> DeleteProjectAsync(string id);
        Task<ProjectResult<IEnumerable<Project>>> GetProjectsAsync();
        Task<ProjectResult<Project>> GetProjectsAsync(string id);
        Task<ProjectResult> UpdateProjectAsync(EditProjectDto dto);
        Task<ProjectResult> UpdateProjectMembersAsync(string projectId, string selectedMemberIdsJson);
    }
}