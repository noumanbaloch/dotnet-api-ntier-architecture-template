using Breeze.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Breeze.DbCore.Context;

public class DatabaseContext : IdentityDbContext<IdentityUser<int>, IdentityRole<int>, int>, IDatabaseContext
{
    public DatabaseContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.Property(e => e.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken();
        });

        ConfigureRowVersionForEntities(modelBuilder);

        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    private void ConfigureRowVersionForEntities(ModelBuilder modelBuilder)
    {
        var entityTypes = modelBuilder.Model.GetEntityTypes()
            .Where(e => typeof(BaseEntity).IsAssignableFrom(e.ClrType));

        foreach (var entityType in entityTypes)
        {
            modelBuilder.Entity(entityType.ClrType)
                .Property("RowVersion")
                .IsRowVersion()
                .IsConcurrencyToken();
        }
    }

    public DbSet<UserEntity> UserEntities { get; set; }
    public DbSet<BoardDetailEntity> BoardEntities { get; set; }
    public DbSet<OTPCodeEntity> OTPCodeEntities { get; set; }
    public DbSet<CollegeEntity> CollegeEntities { get; set; }
    public DbSet<LogEntryErrorEntity> LogEntryErrorEntities { get; set; }
}