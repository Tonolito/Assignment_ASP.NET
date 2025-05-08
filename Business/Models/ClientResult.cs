using Domain.Models;

namespace Business.Models;

public class ClientResult : ServiceResult
{
    public Client Result { get; set; } = new();
}
