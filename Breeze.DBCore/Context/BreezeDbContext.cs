using Breeze.Models.Constants;
using Breeze.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Breeze.DbCore.Context;

public class BreezeDbContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>, IBreezeDbContext
{
    public BreezeDbContext(DbContextOptions<BreezeDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder
       .Entity<UserEntity>()
       .ToTable(TableNames.USERS_TABLE);

        builder
       .Entity<BoardDetailEntity>()
       .ToTable(TableNames.BOARD_DETAILS_TABLE)
       .HasQueryFilter(e => !e.Deleted);

        builder
       .Entity<OTPCodeEntity>()
       .ToTable(TableNames.OTP_CODES_TABLE)
       .HasQueryFilter(e => !e.Deleted);

        builder
       .Entity<CollegeEntity>()
       .ToTable(TableNames.COLLEGES_TABLE)
       .HasQueryFilter(e => !e.Deleted);

        builder
        .Entity<LogEntryErrorEntity>()
        .ToTable(TableNames.LOG_ENTRY_ERRORS_TABLE)
        .HasQueryFilter(e => !e.Deleted);

        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<UserEntity> UserEntities => Set<UserEntity>();
    public DbSet<BoardDetailEntity> BoardEntities => Set<BoardDetailEntity>();
    public DbSet<OTPCodeEntity> OTPCodeEntities => Set<OTPCodeEntity>();
    public DbSet<CollegeEntity> CollegeEntities => Set<CollegeEntity>();
    public DbSet<LogEntryErrorEntity> LogEntryErrorEntities => Set<LogEntryErrorEntity>();
}