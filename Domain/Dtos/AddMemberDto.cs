

using Microsoft.AspNetCore.Http;

namespace Domain.Dtos;

public class AddMemberDto
{
    public IFormFile? MemberImage { get; set; }


    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;


    public string? Phone { get; set; }


    public string JobTitle { get; set; } = null!;


    public string? Address { get; set; }


    public DateOnly? DateOfBirth { get; set; }
}
