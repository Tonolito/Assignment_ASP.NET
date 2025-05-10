
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class ProjectMemberEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [ForeignKey("ProjectId")]
    public string ProjectId { get; set; } = null!;
    public ProjectEntity Project { get; set; } = null!;

    public string MemberId { get; set; } = null!;
    public MemberEntity Member { get; set; } = null!;
}

