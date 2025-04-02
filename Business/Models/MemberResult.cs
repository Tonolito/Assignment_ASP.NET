using Domain.Models;

namespace Business.Models;

public class MemberResult : ServiceResult
{
    public IEnumerable<Member>? Result { get; set; }
}
