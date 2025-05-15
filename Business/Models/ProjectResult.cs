using Domain.Models;

namespace Business.Models;

public class ProjectResult : ServiceResult
{
    public Project Result { get; set; } = new();

}

