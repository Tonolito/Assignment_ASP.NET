using Domain.Models;

namespace WebApp.ViewModels;

public class MembersViewModel
{

    public IEnumerable<MemberViewModel> Members { get; set; } = [];

    public AddMemberViewModel AddMemberViewModel { get; set; } = null!;

}
