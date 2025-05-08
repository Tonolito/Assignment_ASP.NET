using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<MemberEntity>(options)
{
    public virtual DbSet<MemberAddressEntity> MemberAddresses { get; set; } = null!;
    public virtual DbSet<ClientEntity> Clients { get; set; } = null!;

    public virtual DbSet<ProjectEntity> Projects { get; set; } = null!;

    public virtual DbSet<StatusEntity> Statuses { get; set; } = null!;



    public virtual DbSet<NotificationEntity> Notifications { get; set; } = null!;

    public virtual DbSet<NotificationDismissedEntity> DismissedNotifications { get; set; } = null!;

    public virtual DbSet<NotificationTypeEntity> NotificationTypes { get; set; } = null!;

    public virtual DbSet<NotificationTargetGroupEntity> NotificationTargetGroup { get; set; } = null!;

    public virtual DbSet<TagEntity> Tags { get; set; } = null!;

    public virtual DbSet<ProjectMemberEntity> ProjectMembers { get; set; } = null!;


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Prevent cascade delete from ProjectMemberEntity to MemberEntity
        builder.Entity<ProjectMemberEntity>()
            .HasOne(pm => pm.Member)
            .WithMany(m => m.ProjectMembers)
            .HasForeignKey(pm => pm.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        // (Optional but recommended) Explicitly set delete behavior for ProjectEntity.Member too
        builder.Entity<ProjectEntity>()
            .HasOne(p => p.Member)
            .WithMany(m => m.Projects)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }

}


