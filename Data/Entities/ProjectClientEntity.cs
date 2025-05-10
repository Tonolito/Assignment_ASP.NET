using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class ProjectClientEntity
{

    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    public string ProjectId { get; set; } = null!;
    public ProjectEntity Project { get; set; } = null!;

    public string ClientId { get; set; } = null!;
    public ClientEntity Client { get; set; } = null!;
}

