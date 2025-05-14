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

    [ForeignKey(nameof(Status))]
    public int StatusId { get; set; }
    public StatusEntity Status { get; set; } = null!;

    public ICollection<ProjectMemberEntity> ProjectMembers { get; set; } = [];

    //public virtual ICollection<ProjectClientEntity> ProjectClients { get; set; } = [];


}

