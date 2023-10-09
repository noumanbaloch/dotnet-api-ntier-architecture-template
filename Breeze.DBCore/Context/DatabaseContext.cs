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
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<UserEntity> UserEntities { get; set; }
    public DbSet<BoardDetailEntity> BoardEntities { get; set; }
    public DbSet<OTPCodeEntity> OTPCodeEntities { get; set; }
    public DbSet<CollegeEntity> CollegeEntities { get; set; }
}