using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<MemberEntity>(options)
{

    // ALLA EGNA TABLLER

    public virtual DbSet<MemberAddressEntity> MemberAddresses { get; set; } = null!;
    public virtual DbSet<ClientEntity> Clients { get; set; } = null!;

    public virtual DbSet<ProjectEntity> Projects { get; set; } = null!;

    public virtual DbSet<StatusEntity> Statuses { get; set; } = null!;

    public virtual DbSet<NotificationEntity> Notifications { get; set; } = null!;

    public virtual DbSet<NotificationDismissedEntity> DismissedNotifications { get; set; } = null!;

    public virtual DbSet<NotificationTypeEntity> NotificationTypes { get; set; } = null!;
    public virtual DbSet<NotificationTargetGroupEntity> NotificationTargetGroup { get; set; } = null!;

    //https://youtu.be/P8ycDmoMFy4
}
