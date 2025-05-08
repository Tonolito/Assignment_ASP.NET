using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class ProjectEntity
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
     public string? Image { get; set; }
    public string? ProjectName { get; set; }

    public string? Description { get; set; }

    [Column(TypeName ="date")]
    public DateTime StartDate { get; set; }

    [Column(TypeName = "date")]
    public DateTime? EndDate { get; set; }

    public decimal? Budget {  get; set; }

    public DateTime Created { get; set; }  = DateTime.Now;

    [ForeignKey(nameof(Client))]
    public string ClientId { get; set; } = null!;
    public ClientEntity Client { get; set; } =null!;

    [ForeignKey(nameof(Member))]
    public string UserId { get; set; } = null!;
    public MemberEntity Member { get; set; } = null!;

    [ForeignKey(nameof(Status))]
    public int StatusId { get; set; }
    public StatusEntity Status { get; set; } = null!;

    public virtual ICollection<ProjectMemberEntity> ProjectMembers { get; set; } = [];

}

