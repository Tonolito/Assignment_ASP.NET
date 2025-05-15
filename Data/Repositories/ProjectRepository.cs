using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Models;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;

namespace Data.Repositories;

public class ProjectRepository(AppDbContext context) : BaseRepository<ProjectEntity, Project>(context), IProjectRepository
{
    public virtual async Task<RepositoryResult<IEnumerable<Project>>> GetAllProjectsAsync(bool orderByDescending = false, Expression<Func<ProjectEntity, object>>? sortBy = null, Expression<Func<ProjectEntity, bool>>? where = null, params Expression<Func<ProjectEntity, object>>[] includes)
    {
        IQueryable<ProjectEntity> query = _table;

        if (where != null)
        {
            query = query.Where(where);
        }
        if (includes != null && includes.Length != 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }
        if (sortBy != null)
        {
            query = orderByDescending
                ? query.OrderByDescending(sortBy)
                : query.OrderBy(sortBy);
        }

        var entities = await query.ToListAsync();

        // Mappa varje ProjectEntity till Project-modell här
        var result = entities.Select(entity => new Project
        {
            Id = entity.Id,
            Image = entity.Image,
            ProjectName = entity.ProjectName,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Budget = entity.Budget,
            StatusId = entity.StatusId,  // Mappa StatusId direkt från ProjectEntity
            MemberIds = entity.ProjectMembers?.Select(pm => pm.MemberId.ToString()).ToList() ?? new List<string>(), // Mappa ProjectMembers till MemberIds
            Client = entity.Client != null
            ? new Client
            {
                Id = entity.Client.Id,
                ClientName = entity.Client.ClientName
            }
            : null
        });

        return new RepositoryResult<IEnumerable<Project>> { Succeeded = true, StatusCode = 200, Result = result };
    }

    public virtual async Task<RepositoryResult<Project>> GetProjectByIdAsync(
    string id,
    params Expression<Func<ProjectEntity, object>>[] includes)
    {
        IQueryable<ProjectEntity> query = _table;

        // Lägg till includes (t.ex. ProjectMembers, Status)
        if (includes != null && includes.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        var entity = await query.FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new RepositoryResult<Project> { Succeeded = false, StatusCode = 404 };
        }

        // Mappa ProjectEntity till Project
        var project = new Project
        {
            Id = entity.Id,
            Image = entity.Image,
            ProjectName = entity.ProjectName,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Budget = entity.Budget,
            StatusId = entity.StatusId,
            MemberIds = entity.ProjectMembers?.Select(pm => pm.MemberId.ToString()).ToList() ?? new List<string>()
        };

        return new RepositoryResult<Project> { Succeeded = true, StatusCode = 200, Result = project };
    }

}
