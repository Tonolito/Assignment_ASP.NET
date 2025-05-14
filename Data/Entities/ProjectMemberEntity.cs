
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class ProjectMemberEntity
{
    
    public string? ProjectId { get; set; } 
    public ProjectEntity Project { get; set; } = null!;

    public string? MemberId { get; set; } 
    public MemberEntity Member { get; set; } = null!;
}

