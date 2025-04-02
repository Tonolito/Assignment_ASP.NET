using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Domain.Dtos;
using Domain.Extentions;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public interface IAuthService
{
    Task<AuthResult> SignInAsync(MemberSignInDto dto);
    Task<AuthResult> SignOutAsync();
    Task<AuthResult> SignUpAsync(MemberSignUpDto dto);
}

public class AuthService(SignInManager<MemberEntity> signInManager, UserManager<MemberEntity> userManager, IMemberService memberService) : IAuthService
{
    private readonly SignInManager<MemberEntity> _signInManager = signInManager;
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly IMemberService _memberService = memberService;

    public async Task<AuthResult> SignInAsync(MemberSignInDto dto)
    {
        if (dto == null)
        {
            return new AuthResult { Succeeded = false, StatusCode = 400, Error = "Not all requried fields is filled" };
        }

        var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);

        return result.Succeeded
            ? new AuthResult { Succeeded = true, StatusCode = 200 }
            : new AuthResult { Succeeded = false, StatusCode = 401, Error = "Invalid email or password" };
    }

    public async Task<AuthResult> SignUpAsync(MemberSignUpDto dto)
    {

        if (dto == null)
        {
            return new AuthResult { Succeeded = false, StatusCode = 400, Error = "Not all requried fields is filled" };
        }

        //var memberEntity = new MemberEntity
        //{
        //    UserName = dto.Email,
        //    Email = dto.Email,
        //    FirstName = dto.FirstName,
        //    LastName = dto.LastName,

        //};
        //var result = await _memberService.CreateMemberAsync(dto);

        var entity = dto.MapTo<MemberEntity>();
        entity.UserName = dto.Email;
        
        var result = await _userManager.CreateAsync(entity, dto.Password);


        return result.Succeeded
        ? new AuthResult { Succeeded = true, StatusCode = 201 }
        : new AuthResult { Succeeded = false, StatusCode = 999, Error = "FAILED" };

        

    }

    public async Task<AuthResult> SignOutAsync()
    {
        try
        {
            await _signInManager.SignOutAsync();
            return new AuthResult { Succeeded = true, StatusCode = 200 }; 
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return new AuthResult { Succeeded = false, StatusCode = 500, Error = "Failed to log out" };
        }
    }
}
