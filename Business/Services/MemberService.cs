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
using Data.Contexts;

namespace Business.Services;

public class MemberService(UserManager<MemberEntity> userManager, IMemberRepository memberRepository, RoleManager<IdentityRole> roleManger, AppDbContext context) : IMemberService
{
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly IMemberRepository _memberRepository = memberRepository;
    private readonly RoleManager<IdentityRole> _roleManger = roleManger;
    private readonly AppDbContext _context = context;



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
            memberEntity.Image = "/images/avatars/templateavatar.svg";

            Console.WriteLine("Image: " + memberEntity.Image);

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
            // Hjäp av chatgtp
            string imageUrl = string.Empty;
            if (dto.MemberImage != null && dto.MemberImage.Length > 0)
            {

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(dto.MemberImage.FileName)}";


                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "avatars");


                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);


                var filePath = Path.Combine(folderPath, fileName);


                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.MemberImage.CopyToAsync(stream);
                }

                imageUrl = $"/images/avatars/{fileName}";
            }
            else
            {
                imageUrl = $"/images/avatars/templateavatar.svg";
            }

            var entity = dto.MapTo<MemberEntity>();
            entity.UserName = dto.Email;
            entity.Image = imageUrl;


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
    public async Task<MembersResult> GetMembersAsnyc()
    {

        var result = await _memberRepository.GetAllAsync();

        return result.MapTo<MembersResult>();


    }
    public async Task<MemberResult> GetMemberByIdAsync(string id)
    {
        var result = await _memberRepository.GetAsync(x => x.Id == id);

        return result.MapTo<MemberResult>();

    }

    public async Task<string?> GetMemberImageAsync(string username)
    {
        var member = await _userManager.FindByNameAsync(username);

        if (member == null)
        {
            return null;
        }

        return member.Image ?? "/images/avatars/templateavatar.svg";
    }


    //UPDATE
        public async Task<MemberResult> EditMemberAsync(EditMemberDto dto)
        {

        var existing = await _context.Users.FindAsync(dto.Id); // om du jobbar i service-lagret med context
        if (existing == null)
        {
            return new MemberResult { Succeeded = false, StatusCode = 404, Error = "Member not found" };
        }
            existing.Id = dto.Id;
            //existing.Image = dto.MemberImage;
            existing.FirstName = dto.FirstName;
            existing.LastName = dto.LastName;
            existing.Email = dto.Email;
            existing.PhoneNumber = dto.PhoneNumber;
            existing.JobTitle = dto.JobTitle;
            //existing.Address = dto.Address;



        var result = await _memberRepository.UpdateAsync(existing);

            if (result.Succeeded)
            {
                return result.Succeeded
                ? new MemberResult { Succeeded = true, StatusCode = 201, }
                : new MemberResult { Succeeded = false, StatusCode = 201, Error = "Member update but sometig went wrong" };
            }
            
            return new MemberResult { Succeeded = false, StatusCode = 500, Error = "Unable to edit member" };

        }

    // DELETE
    //Hjäp av chatgpt för den tidigare delen krockade med att läsa entiteten två gånger
    public async Task<MemberResult> DeleteMemberAsync(string id)
    {
        if (id == null)
        {
            return new MemberResult { Succeeded = false, StatusCode = 400, Error = "Id can't be found." };
        }
        try
        {
            var memberEntity = await _context.Users.FindAsync(id);

            if (memberEntity == null)
            {
                return new MemberResult { Succeeded = false, StatusCode = 404, Error = "Member not found." };
            }

            // Ta bort medlemmen
            _context.Users.Remove(memberEntity);
            await _context.SaveChangesAsync();

            return new MemberResult { Succeeded = true, StatusCode = 200 };
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
            return new MemberResult { Succeeded = false, StatusCode = 500, Error = ex.Message };
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

    public async Task<List<MemberSearchDto>> SearchMemberAsync(string term)
    {
        if (string.IsNullOrEmpty(term))
            return new List<MemberSearchDto>();

        var users = await _userManager.Users
            .Where(x => x.FirstName.Contains(term) || x.LastName.Contains(term) || x.Email.Contains(term))
            .Select(x => new MemberSearchDto
            {
                Id = x.Id,
                FullName = x.FirstName + " " + x.LastName,
                ImageUrl = x.Image,
            })
            .ToListAsync();

        return users;

    }



}
