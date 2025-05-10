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

    public virtual DbSet<ProjectClientEntity> ProjectClients { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Define composite key for ProjectClientEntity
        modelBuilder.Entity<ProjectClientEntity>()
            .HasKey(pc => pc.Id);  // `Id` as the primary key

        // Define the relationships with foreign keys explicitly
        modelBuilder.Entity<ProjectClientEntity>()
            .HasOne(pc => pc.Project)
            .WithMany(p => p.ProjectClients)  // Assuming Project has a collection of ProjectClientEntity
            .HasForeignKey(pc => pc.ProjectId)
            .OnDelete(DeleteBehavior.Cascade); // Or NoAction, depending on your business rules

        modelBuilder.Entity<ProjectClientEntity>()
            .HasOne(pc => pc.Client)
            .WithMany(c => c.ProjectClients)  // Assuming Client has a collection of ProjectClientEntity
            .HasForeignKey(pc => pc.ClientId)
            .OnDelete(DeleteBehavior.Cascade); // Or NoAction
    }

}


