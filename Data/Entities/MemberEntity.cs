using Microsoft.AspNetCore.Identity;

namespace Data.Entities;

public class MemberEntity : IdentityUser
{
    public string? Image { get; set; }

    [ProtectedPersonalData]
    public string? FirstName { get; set; }

    [ProtectedPersonalData]

    public string? LastName { get; set; }

    [ProtectedPersonalData]

    public string? JobTitle { get; set; }

    public int AddressId { get; set; }
    public MemberAddressEntity? Address { get; set; }

    public ICollection<NotificationDismissedEntity> DismissedNotifications { get; set; } = [];

    public  ICollection<ProjectMemberEntity> ProjectMembers { get; set; } = [];

}
