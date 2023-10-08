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
    public DbSet<AcademicDisciplineEntity> AcademicDisciplineEntities { get; set; }
    public DbSet<OTPCodeEntity> OTPCodeEntities { get; set; }
    public DbSet<CollegeEntity> CollegeEntities { get; set; }
    public DbSet<StudentEntity> StudentEntities { get; set; }
    public DbSet<SubjectEntity> SubjectEntities { get; set; }
    public DbSet<ChapterEntity> ChapterEntities { get; set; }
    public DbSet<CardEntity> CardEntities { get; set; }
    public DbSet<StudentCardEntity> StudentCardEntities { get; set; }
    public DbSet<CardPageEntity> CardPageEntities { get; set; }
    public DbSet<CardTypeEntity> CardTypeEntities { get; set; }
    public DbSet<SubscriptionEntity> SubscriptionEntities { get; set; }
    public DbSet<StudentGoalEntity> StudentGoalEntities { get; set; }
    public DbSet<ReportedCardEntity> ReportedCardEntities { get; set; }
    public DbSet<PaymentReciptEntity> PaymentReciptEntities { get; set; }
    public DbSet<VerifiedReciptEntity> VerifiedReciptEntities { get; set; }
    public DbSet<SubscriptionPlanEntity> SubscriptionPlanEntities { get; set; }
    public DbSet<BankDetailEntity> BankDetailEntities { get; set; }
    public DbSet<ApplicationUpdateEntity> ApplicantionUpdateEntities { get; set; }
    public DbSet<ReferralEntity> ReferralEntities { get; set; }
}