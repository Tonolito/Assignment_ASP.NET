using Data.Entities;
using Data.Models;
using Domain.Models;
using System.Linq.Expressions;

namespace Data.Interfaces;

public interface IProjectRepository : IBaseRepository<ProjectEntity, Project>
{
    Task<RepositoryResult<IEnumerable<Project>>> GetAllProjectsAsync(bool orderByDescending = false, Expression<Func<ProjectEntity, object>>? sortBy = null, Expression<Func<ProjectEntity, bool>>? where = null, params Expression<Func<ProjectEntity, object>>[] includes);
    Task<RepositoryResult<Project>> GetProjectByIdAsync(string id, params Expression<Func<ProjectEntity, object>>[] includes);
}
