using Data.Entities;
using Domain.Models;

namespace Data.Interfaces;

public interface IProjectRepository : IBaseRepository<ProjectEntity, Project>
{
    Task<ProjectEntity?> GetProjectWithDetailsAsync(string projectId);
}
