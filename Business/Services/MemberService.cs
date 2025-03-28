using Domain.Dtos;
using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public interface IMemberService
{
    Task<bool> AddMemberAsync(AddMemberDto dto);
    Task<bool> EditMemberAsync(EditMemberDto dto);
    Task<IEnumerable<Member>> GetAllMembersAsync();
}

public class MemberService : IMemberService
{
    private readonly UserManager<MemberEntity> _userManager;

    public MemberService(UserManager<MemberEntity> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<bool> AddMemberAsync(AddMemberDto dto)
    {
        //FACTORY
        var addMemberEntity = new MemberEntity
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            JobTitle = dto.JobTitle,
            PhoneNumber = dto.Phone,
 
        };
        var result = await _userManager.CreateAsync(addMemberEntity, "BytMig123!");
        return result.Succeeded;

    }
    public async Task<bool> EditMemberAsync(EditMemberDto dto)
    {
        //FACTORY
        var editMemberEntity = new MemberEntity
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            JobTitle = dto.JobTitle,
            PhoneNumber = dto.Phone,

        };
        var result = await _userManager.UpdateAsync(editMemberEntity);
        return result.Succeeded;

    }

    public async Task<IEnumerable<Member>> GetAllMembersAsync()
    {
        var entities = await _userManager.Users.ToListAsync();

        var members = entities.Select(x => new Member
        {
            // FACTORY Omvandlar från memberENtity till member model
            Id = x.Id,
            FirstName = x.FirstName,
            LastName = x.LastName,
            Email = x.Email,
            Phone = x.PhoneNumber,
            JobTitle = x.JobTitle,
            //Address = x.Address,
        });
        return members;
    }
}
