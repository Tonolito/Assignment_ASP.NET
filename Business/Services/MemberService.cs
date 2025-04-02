using Domain.Dtos;
using Data.Entities;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Business.Models;
using Data.Interfaces;
using Domain.Extentions;
using Business.Interfaces;
using System.Diagnostics;
using Data.Repositories;

namespace Business.Services;

public class MemberService(UserManager<MemberEntity> userManager, IMemberRepository memberRepository, RoleManager<IdentityRole> roleManger) : IMemberService
{
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly RoleManager<IdentityRole> _roleManger = roleManger;



    //CREATE

    /// <summary>
    /// It's for signinUp
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public async Task<MemberResult> CreateMemberAsync(MemberSignUpDto dto, string roleName = "User")
    {
        if (dto == null)
        {
            return new MemberResult { Succeeded = false, StatusCode = 400, Error = "Dto can't be null" };
        }


        var existsResult = await _memberRepository.ExistsAsync(x => x.Email == dto.Email);

        if (existsResult.Succeeded)
        {
            return new MemberResult { Succeeded = false, StatusCode = 409, Error = "Member with same email exists" };

        }

        try
        {
            var memberEntity = dto.MapTo<MemberEntity>();
            memberEntity.UserName = dto.Email;


            var result = await _userManager.CreateAsync(memberEntity, dto.Password);


            if (result.Succeeded)
            {
                var addToRoleResult = await AddMemberToRole(memberEntity.Id, roleName);
                return result.Succeeded
            ? new MemberResult { Succeeded = true, StatusCode = 201, }
            : new MemberResult { Succeeded = false, StatusCode = 201, Error = "Member created but not to role" };
            }

            return new MemberResult { Succeeded = false, StatusCode = 500, Error = "Unable to add member" };


        }
        catch (Exception ex)
        {
            Debug.Write(ex);
            return new MemberResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }

    }

    /// <summary>
    /// It's for adding a member with a already set password
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public async Task<MemberResult> AddMemberAsync(AddMemberDto dto, string roleName = "User")
    {
        if (dto == null)
        {
            return new MemberResult { Succeeded = false, StatusCode = 400, Error = "Dto can't be null" };
        }


        var existsResult = await _memberRepository.ExistsAsync(x => x.Email == dto.Email);

        if (existsResult.Succeeded)
        {
            return new MemberResult { Succeeded = false, StatusCode = 409, Error = "Member with same email exists" };

        }

        try
        {

            var entity = dto.MapTo<MemberEntity>();
            // VIKTIG
            entity.UserName = dto.Email;


            var result = await _userManager.CreateAsync(entity, "BytMig123!");

            if (result.Succeeded)
            {
                var addToRoleResult = await AddMemberToRole(entity.Id, roleName);
                return result.Succeeded
            ? new MemberResult { Succeeded = true, StatusCode = 201, }
            : new MemberResult { Succeeded = false, StatusCode = 201, Error = "Member created but not to role" };
            }

            return new MemberResult { Succeeded = false, StatusCode = 500, Error = "Unable to add member" };


        }
        catch (Exception ex)
        {
            Debug.Write(ex);
            return new MemberResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
        }

    }

    //READ
    public async Task<MemberResult> GetMembersAsnyc()
    {

        var result = await _memberRepository.GetAllAsync();

        return result.MapTo<MemberResult>();


    }
    public async Task<MemberResult> GetMemberByIdAsync(string id)
    {
        var result = await _memberRepository.GetAsync(x => x.Id == id);

        return result.MapTo<MemberResult>();

    }

    //UPDATE
    public async Task<MemberResult> EditMemberAsync(EditMemberDto dto)
    {
        var entity = dto.MapTo<MemberEntity>();

        var result = await _memberRepository.UpdateAsync(entity);

        if (result.Succeeded)
        {
            return result.Succeeded
            ? new MemberResult { Succeeded = true, StatusCode = 201, }
            : new MemberResult { Succeeded = false, StatusCode = 201, Error = "Member update but sometig went wrong" };
        }

        return new MemberResult { Succeeded = false, StatusCode = 500, Error = "Unable to edit member" };

    }

    // DELETE
    public async Task<MemberResult> DeleteMemberAsync(string id)
    {
        if (id == null)
        {
            return new MemberResult { Succeeded = false, StatusCode = 400, Error = "Id can't be found." };

        }
        try
        {
            var memberResult = await _memberRepository.GetAsync(x => x.Id == id);
            var memberEntity = memberResult.Result!.MapTo<MemberEntity>();

            var result = await _memberRepository.DeleteAsync(memberEntity);
            return result.Succeeded
            ? new MemberResult { Succeeded = true, StatusCode = 200 }
            : new MemberResult { Succeeded = false, StatusCode = result.StatusCode, Error = result.Error };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return new MemberResult { Succeeded = false,StatusCode = 500, Error = ex.Message };
        }
    }

    // MSC
    public async Task<MemberResult> AddMemberToRole(string userId, string roleName)
    {
        if (!await _roleManger.RoleExistsAsync(roleName))
        {
            return new MemberResult { Succeeded = false, Error = "Roles doesn't exists" };
        }

        var member = await _userManager.FindByIdAsync(userId);
        if (member == null)
        {
            return new MemberResult { Succeeded = false, StatusCode = 404, Error = "User doesn't exists" };
        }

        var result = await _userManager.AddToRoleAsync(member, roleName);
        return result.Succeeded
        ? new MemberResult { Succeeded = true, StatusCode = 200 }
        : new MemberResult { Succeeded = false, StatusCode = 500, Error = "Unable to add user" };
    }

}
