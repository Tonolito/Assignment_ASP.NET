using Domain.Dtos;
using Domain.Models;
using Business.Models;

namespace Business.Interfaces;

public interface IMemberService
{
    Task<MemberResult> AddMemberAsync(AddMemberDto dto, string roleName = "User");
    Task<MemberResult> AddMemberToRole(string userId, string roleName);
    Task<MemberResult> CreateMemberAsync(MemberSignUpDto dto, string roleName = "User");
    Task<MemberResult> EditMemberAsync(EditMemberDto dto);
    Task<MemberResult> GetMemberByIdAsync(string id);
    Task<MemberResult> GetMembersAsnyc();
}
