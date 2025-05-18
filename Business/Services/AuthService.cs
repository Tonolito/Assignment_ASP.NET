using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Domain.Dtos;
using Domain.Extentions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
namespace Business.Services;

public interface IAuthService
{
    Task<AuthResult> SignInAsync(MemberSignInDto dto);
    Task<AuthResult> SignOutAsync();
    Task<AuthResult> SignUpAsync(MemberSignUpDto dto, string roleName = "User");
}

public class AuthService(SignInManager<MemberEntity> signInManager, UserManager<MemberEntity> userManager, IMemberService memberService, INotificationService notificationService) : IAuthService
{
    private readonly SignInManager<MemberEntity> _signInManager = signInManager;
    private readonly UserManager<MemberEntity> _userManager = userManager;
    private readonly IMemberService _memberService = memberService;
    private readonly INotificationService _notificationService = notificationService;

    /// <summary>
    /// Check if Mebember is a user with a log in and creates a notification
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<AuthResult> SignInAsync(MemberSignInDto dto)
    {
        if (dto == null)
        {
            return new AuthResult { Succeeded = false, StatusCode = 400, Error = "Not all requried fields is filled" };
        }

        var result = await _signInManager.PasswordSignInAsync(dto.Email, dto.Password, false, false);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user != null)
            {
                await AddClaimByEmailAsync(user, "DisplayName", $"{user.FirstName} {user.LastName}");
                var notificationEntity = new NotificationEntity()
                {
                    Message = $"{user.FirstName} {user.LastName} signed in.",
                    NotificationTypeId = 1,
                    Icon = user.Image,
                };
                await _notificationService.AddNotificitionAsync(notificationEntity, user.Id);
            }
        }
        return result.Succeeded
            ? new AuthResult { Succeeded = true, StatusCode = 200 }
            : new AuthResult { Succeeded = false, StatusCode = 401, Error = "Invalid email or password" };
    }

    public async Task AddClaimByEmailAsync(MemberEntity user, string typeName, string value)
    {
        if (user != null)
        {

            var claims = await _userManager.GetClaimsAsync(user);

            if (!claims.Any(x => x.Type == typeName))
            {
                await _userManager.AddClaimAsync(user, new Claim(typeName, value));
            }

        }
    }


    /// <summary>
    /// Creates a Member with the role User.
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="roleName"></param>
    /// <returns></returns>
    public async Task<AuthResult> SignUpAsync(MemberSignUpDto dto, string roleName = "User")
    {

        if (dto == null)
        {
            return new AuthResult { Succeeded = false, StatusCode = 400, Error = "Not all requried fields is filled" };
        }

        

        var entity = dto.MapTo<MemberEntity>();
        entity.UserName = dto.Email;
        entity.Image = "/images/avatars/templateavatar.svg";
        var result = await _userManager.CreateAsync(entity, dto.Password);

        if (result.Succeeded)
        {
            var addToRoleResult = await _memberService.AddMemberToRole(entity.Id, roleName);
            return result.Succeeded
        ? new AuthResult { Succeeded = true, StatusCode = 201, }
        : new AuthResult { Succeeded = false, StatusCode = 201, Error = "Member created but not to role" };
        }

        return result.Succeeded
        ? new AuthResult { Succeeded = true, StatusCode = 201 }
        : new AuthResult { Succeeded = false, StatusCode = 999, Error = "FAILED" };



    }
    /// <summary>
    /// Makes it possible to log out
    /// </summary>
    /// <returns></returns>
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
