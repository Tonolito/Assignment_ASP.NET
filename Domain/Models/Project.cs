
namespace Domain.Models;

public class Project
{
    public string? Id { get; set; } 
    public string? Image { get; set; }
    public string? ProjectName { get; set; }

    public string? Description { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public decimal? Budget { get; set; }

    public List<string> MemberIds { get; set; } = new();

    public List<Member> Members { get; set; } = [];

    public string? ClientId { get; set; }
    public Client? Client { get; set; } 
    public int? StatusId { get; set; }
}
