﻿using Data.Contexts;
using Data.Entities;
using Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace Data.Repositories
{
    public class ProjectMemberRepository(AppDbContext context) : BaseRepository<ProjectMemberEntity, Tag>(context), IProjectMemberRepository
    {
       

      

        public async Task RemoveAllByProjectIdAsync(string projectId)
        {
            var members = await _context.ProjectMembers
                .Where(pm => pm.ProjectId == projectId)
                .ToListAsync();

            _context.ProjectMembers.RemoveRange(members);
            await _context.SaveChangesAsync();
        }

        public async Task AddMembersAsync(string projectId, List<string> memberIds)
        {
            var newMembers = memberIds.Select(id => new ProjectMemberEntity
            {
                ProjectId = projectId,
                MemberId = id
            });

            _context.ProjectMembers.AddRange(newMembers);
            await _context.SaveChangesAsync();
        }
    }

    public interface IProjectMemberRepository
    {
        Task AddMembersAsync(string projectId, List<string> memberIds);
        Task RemoveAllByProjectIdAsync(string projectId);
    }
}
