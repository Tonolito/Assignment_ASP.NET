﻿using Data.Entities;
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

        
    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    base.OnModelCreating(builder);

    //    builder.Entity<ProjectMemberEntity>()
    //        .HasKey(pm => new { pm.ProjectId, pm.MemberId });

    //    builder.Entity<ProjectMemberEntity>()
    //        .HasOne(pm => pm.Project)
    //        .WithMany(p => p.ProjectMembers)
    //        .HasForeignKey(pm => pm.ProjectId);

    //    builder.Entity<ProjectMemberEntity>()
    //        .HasOne(pm => pm.Member)
    //        .WithMany(m => m.ProjectMembers)
    //        .HasForeignKey(pm => pm.MemberId);
    //}
}


