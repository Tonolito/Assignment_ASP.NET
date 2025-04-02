﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

[Index(nameof(ClientName), IsUnique = true)]
public class ClientEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string? Image { get; set; }

    public string ClientName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Location { get; set; }

    public string? Phone { get; set; }

    public DateTime Created { get; set; } = DateTime.Now;

    public virtual ICollection<ProjectEntity> Projects { get; set; } = [];
}
