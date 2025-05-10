using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProjectRepository(AppDbContext context) : BaseRepository<ProjectEntity, Project>(context), IProjectRepository
{
    // ProjectRepository.cs (or similar repository file)
    public async Task<ProjectEntity?> GetProjectWithDetailsAsync(string projectId)
    {
        var project = await _context.Projects
            .Include(p => p.ProjectMembers)
            .Include(p => p.ProjectClients)
            .FirstOrDefaultAsync(p => p.Id == projectId);

        return project;
    }

}
