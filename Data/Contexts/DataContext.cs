﻿using Data.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<MemberEntity>(options)
{

    // ALLA EGNA TABLLER

    public virtual DbSet<MemberAddressEntity> MemberAddresses { get; set; } = null!;
}
