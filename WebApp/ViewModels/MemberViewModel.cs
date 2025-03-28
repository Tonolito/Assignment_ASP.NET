using Domain.Models;

namespace WebApp.ViewModels;

public class MemberViewModel
{
    public Member Member { get; set; } = null!;
    public EditMemberViewModel EditMemberViewModel { get; set; } = null!;
}
