﻿namespace Domain.Models;

public class Member
{
    public string? Id { get; set; }

    public string? Image { get; set; } 

    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? JobTitle { get; set; }
    public string Email { get; set; } = null!;
    public string? PhoneNumber { get; set; }
    
    public MemberAddress? Address { get; set; }
}
