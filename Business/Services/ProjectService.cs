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
using System.Diagnostics;
using System.Text.Json;

namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository, IStatusService statusService, IProjectMemberRepository projectMemberRepository, AppDbContext context, INotificationService notificationService) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IStatusService _statusService = statusService;
    private readonly IProjectMemberRepository _projectMemberRepository = projectMemberRepository;
    private readonly INotificationService _notificationService = notificationService;
    private readonly AppDbContext _context = context;

    //CREATE
    //, List<string> selectedUserIds
    public async Task<ProjectResult> CreateProjectAsync(AddProjectDto dto)
    {
        if (dto == null)
        {
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Not all required fields are input." };
        }

        try
        {
            // Hjäp av chatgtp
            string imageUrl = string.Empty;
            if (dto.ProjectImage != null && dto.ProjectImage.Length > 0)
            {

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.ProjectImage.FileName)}";


                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "projects");


                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);


                var filePath = Path.Combine(folderPath, fileName);


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.ProjectImage.CopyToAsync(stream);
                }

                imageUrl = $"/images/projects/{fileName}";
            }
            else
            {
                imageUrl = $"/images/projects/templateavatar.svg";
            }
            var projectEntity = dto.MapTo<ProjectEntity>();


            projectEntity.Image = imageUrl;
            string? actualClientId = null;

            if (!string.IsNullOrEmpty(dto.SelectedClientId))
            {
                var clientIdList = JsonSerializer.Deserialize<List<string>>(dto.SelectedClientId);
                actualClientId = clientIdList?.FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(actualClientId))
            {
                var client = await _context.Clients.FirstOrDefaultAsync(c => c.Id == actualClientId);
                if (client != null)
                {
                    projectEntity.ClientId = client.Id;
                }
                else
                {
                    return new ProjectResult { Succeeded = false, StatusCode = 404, Error = "Client not found." };
                }
            }
            else
            {
                return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "ClientId is required." };
            }


            var statusResult = await _statusService.GetStatusByIdAsync(1);
            var status = statusResult.Result;

            projectEntity.StatusId = status!.Id;

            var result = await _projectRepository.AddAsync(projectEntity);

            if (result.Succeeded)
            {
                // Skapa kopplinga  r till medlemmar i ProjectMember-tabellen
                if (dto.SelectedMemberIds != null && dto.SelectedMemberIds.Any())
                {
                    // dto.SelectedMemberIds innehåller EN sträng, som i sig är en JSON-lista → deserialisera den
                    var memberIdsJson = dto.SelectedMemberIds[0];  // "[\"id1\", \"id2\"]"
                    var memberIds = JsonSerializer.Deserialize<List<string>>(memberIdsJson);

                    if (memberIds != null && memberIds.Any())
                    {
                        var projectMembers = new List<ProjectMemberEntity>();

                        foreach (var memberId in memberIds)
                        {
                            var member = await _context.Users.FirstOrDefaultAsync(m => m.Id == memberId);

                            if (member != null)
                            {
                                projectMembers.Add(new ProjectMemberEntity
                                {
                                    ProjectId = projectEntity.Id,
                                    Member = member
                                });
                            }
                        }

                        await _context.ProjectMembers.AddRangeAsync(projectMembers);
                        await _context.SaveChangesAsync();
                    }
                }

                var notificationEntity = new NotificationEntity()
                {
                    Message = $"{projectEntity.ProjectName} was created!.",
                    NotificationTypeId = 2,
                    Icon = projectEntity.Image
                };
                await _notificationService.AddNotificitionAsync(notificationEntity, projectEntity.Id);


            }
            return new ProjectResult
            {
                Succeeded = true,
                StatusCode = 201
            };
        }
        catch (Exception ex)
        {
            Debug.Write(ex);
            return new ProjectResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
    }

    //READ - Get all projects with members and clients
    public async Task<ProjectsResult<IEnumerable<Project>>> GetProjectsAsync()
    {
        // 1. Hämta alla projekt med ProjectMembers och Status
        var response = await _projectRepository.GetAllProjectsAsync(
            orderByDescending: true,
            sortBy: s => s.Created,
            where: null,
            include => include.ProjectMembers,
            include => include.Status,
            include => include.Client // ⬅ Lägg till detta

        );

        var projectEntities = response.Result.ToList();
        var projects = response.Result?.ToList() ?? [];

        var clientId = projects
            .Select(p => p.ClientId)
            .Distinct()
            .ToList();

        // 2. Hämta alla unika memberIds från alla projekt
        var allMemberIds = projects
            .SelectMany(p => p.MemberIds)
            .Distinct()
            .ToList();

        // 3. Hämta användare från databasen
        var members = await _context.Users
            .Where(u => allMemberIds.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id);

        var clients = await _context.Clients
            .Where(u => clientId.Contains(u.Id))
            .ToDictionaryAsync(u => u.Id);

        // 4. Fyll på Members-listan i varje projekt
        foreach (var project in projects)
        {
            project.Members = project.MemberIds
                .Where(id => members.ContainsKey(id))
                .Select(id =>
                {
                    var member = members[id];
                    return new Member
                    {
                        Id = member.Id,
                        FirstName = member.FirstName,
                        LastName = member.LastName,
                        Image = member.Image ?? "/images/default-avatar.svg"
                    };
                })
                .ToList();

            
        }

        return new ProjectsResult<IEnumerable<Project>>
        {
            Succeeded = true,
            StatusCode = 200,
            Result = projects
        };

    }


    public async Task<ProjectResult> GetProjectByIdAsync(string id)
    {
        var response = await _projectRepository.GetProjectByIdAsync(
            id,
            include => include.ProjectMembers,
            include => include.Status
        );

        return new ProjectResult
        {
            Succeeded = response.Succeeded,
            StatusCode = response.StatusCode,
            Result = response.Result
        };
    }




    //UPDATE
    public async Task<ProjectResult> UpdateProjectAsync(EditProjectDto dto)
    {
        if (dto == null)
        {
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Invalid project data." };
        }
        var existing = await _projectRepository.GetAsync(x => x.Id == dto.Id);

        //existing.Result.Id = dto.Id;
        //existing.Result.ProjectName = dto.ProjectName;
        //existing.Result.Description = dto.Description;
        //existing.Result.StartDate = dto.StartDate;
        //existing.Result.EndDate = dto.EndDate;
        //existing.Result.Budget = dto.Budget;

        var projectEntity = dto.MapTo<ProjectEntity>();


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
            return new ProjectResult { Succeeded = false, StatusCode = 400, Error = "Id can't be found." };
        }
        try
        {
            var projectEntity = await _context.Projects.FindAsync(id);

            if (projectEntity == null)
            {
                return new ProjectResult { Succeeded = false, StatusCode = 404, Error = "Member not found." };
            }
            _context.Projects.Remove(projectEntity);
            await _context.SaveChangesAsync();

            return new ProjectResult { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return new ProjectResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }
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


