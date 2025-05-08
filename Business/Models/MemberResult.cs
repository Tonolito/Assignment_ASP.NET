using Domain.Models;

namespace Business.Models;

public class MemberResult : ServiceResult
{
    public Member Result { get; set; } = new();
}
