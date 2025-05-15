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

    public  DbSet<ProjectMemberEntity> ProjectMembers { get; set; } = null!;

    //public DbSet<ProjectClientEntity> ProjectClients { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProjectMemberEntity>()
       .HasKey(pm => new
       {
           pm.ProjectId,
           pm.MemberId
       });

        modelBuilder.Entity<ProjectMemberEntity>()
            .HasOne(pm => pm.Project)
            .WithMany(p => p.ProjectMembers)
            .HasForeignKey(pm => pm.ProjectId)
            .OnDelete(DeleteBehavior.Cascade); // Använd rätt DeleteBehavior

        modelBuilder.Entity<ProjectMemberEntity>()
            .HasOne(pm => pm.Member)
            .WithMany(m => m.ProjectMembers)
            .HasForeignKey(pm => pm.MemberId)
            .OnDelete(DeleteBehavior.Cascade); // Eller ett annat DeleteBehavior



        

        modelBuilder.Entity<ProjectEntity>()
    .HasOne(p => p.Client)
    .WithMany(c => c.Projects)
    .HasForeignKey(p => p.ClientId)
    .OnDelete(DeleteBehavior.Cascade); // Valfritt: Cascade eller Restrict

    }



}


