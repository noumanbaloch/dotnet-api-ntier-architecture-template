using Breeze.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Breeze.DbCore.Context;

public class DatabaseContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>, IDatabaseContext
{
    public DatabaseContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder
       .Entity<BoardDetailEntity>()
       .HasQueryFilter(e => !e.Deleted);

        builder
       .Entity<OTPCodeEntity>()
       .HasQueryFilter(e => !e.Deleted);

        builder
       .Entity<CollegeEntity>()
       .HasQueryFilter(e => !e.Deleted);

        builder
        .Entity<LogEntryErrorEntity>()
        .HasQueryFilter(e => !e.Deleted);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<UserEntity> UserEntities { get; set; }
    public DbSet<BoardDetailEntity> BoardEntities { get; set; }
    public DbSet<OTPCodeEntity> OTPCodeEntities { get; set; }
    public DbSet<CollegeEntity> CollegeEntities { get; set; }
    public DbSet<LogEntryErrorEntity> LogEntryErrorEntities { get; set; }
}