﻿using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class MemberSignInDto
{
    [Required]
    [Display(Name ="Email", Prompt ="Enter Email")]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = null!;
    [Required]
    [Display(Name = "Password", Prompt = "Enter Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}
