using Data.Entities;
using Domain.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Business.Services;

public interface IAuthService
{
    Task<bool> SignInAsync(MemberSignInDto dto);
    Task<bool> SignUpAsync(MemberSignUpDto dto);

}

public class AuthService : IAuthService
{
    private readonly SignInManager<MemberEntity> _signInManager;
    private readonly UserManager<MemberEntity> _userManager;

    public AuthService(SignInManager<MemberEntity> signInManager, UserManager<MemberEntity> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }
    public async Task<bool> SignInAsync(MemberSignInDto dto)
    {
        var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);

        return result.Succeeded;
    }

    public async Task<bool> SignUpAsync(MemberSignUpDto dto)
    {
        //FACTORY
        var memberEntity = new MemberEntity
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,

        };

        var result = await _userManager.CreateAsync(memberEntity, dto.Password);

        return result.Succeeded;
    }
}
