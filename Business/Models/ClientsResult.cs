
using Domain.Models;

namespace Business.Models;

public class ClientsResult : ServiceResult
{
    public IEnumerable<Client>? Result { get; set; }
}
