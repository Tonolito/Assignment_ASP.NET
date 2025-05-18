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
    /// <summary>
    /// Includes all the entity with connection tables.
    /// </summary>
    /// <param name="orderByDescending"></param>
    /// <param name="sortBy"></param>
    /// <param name="where"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    /// Help with chatgpt
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

        
        var result = entities.Select(entity => new Project
        {
            Id = entity.Id,
            Image = entity.Image,
            ProjectName = entity.ProjectName,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            Budget = entity.Budget,
            StatusId = entity.StatusId,  
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

    /// <summary>
    /// Includes all the entity with the connection table when searching for a project by id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="includes"></param>
    /// <returns></returns>
    /// Hjälp av chatgpt och Fredrik Nilsson Win24
    public virtual async Task<RepositoryResult<Project>> GetProjectByIdAsync(
    string id,
    params Expression<Func<ProjectEntity, object>>[] includes)
    {
        IQueryable<ProjectEntity> query = _table;

        if (includes != null && includes.Length > 0)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        var includesProjectMembers = includes.Any(i =>
        i.Body is MemberExpression me && me.Member.Name == nameof(ProjectEntity.ProjectMembers));

        if (includesProjectMembers)
        {
            query = query.Include("ProjectMembers.Member");
        }

        var entity = await query.FirstOrDefaultAsync(x => x.Id == id);

        if (entity == null)
        {
            return new RepositoryResult<Project> { Succeeded = false, StatusCode = 404 };
        }

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
            MemberIds = entity.ProjectMembers?.Select(pm => pm.MemberId.ToString()).ToList() ?? new List<string>(),
            Members = entity.ProjectMembers?.Where(pm => pm.Member != null)
            .Select(pm => new Member
            {
                Id = pm.Member.Id,
                FirstName = pm.Member.FirstName,
                LastName = pm.Member.LastName,
                Image = pm.Member.Image
            })
            .ToList() ?? new(),
            ClientId = entity.ClientId,
            Client = entity.Client != null
            ? new Client
            {
                Id = entity.Client.Id,
                ClientName = entity.Client.ClientName,
                Image = entity.Client.Image,
                Email = entity.Client.Email,
                Location = entity.Client.Location,
                Phone = entity.Client.Phone
            }
            : null
        };

        return new RepositoryResult<Project> { Succeeded = true, StatusCode = 200, Result = project };
    }

}
