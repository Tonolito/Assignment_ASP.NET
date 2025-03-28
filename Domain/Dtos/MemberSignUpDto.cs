using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class MemberSignUpDto
{
    
    public string FirstName { get; set; } = null!;

    
    public string LastName { get; set; } = null!;

    
    public string Email { get; set; } = null!;

    
    public string Password { get; set; } = null!;

    
}
