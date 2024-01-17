using Breeze.API.Filter;
using Breeze.API.MappingProfile;
using Breeze.DbCore.Context;
using Breeze.DbCore.UnitOfWork;
using Breeze.Identity;
using Breeze.Services.Auth;
using Breeze.Services.Cache;
using Breeze.Services.ClaimResolver;
using Breeze.Services.DropDown;
using Breeze.Services.Email;
using Breeze.Services.HttpHeader;
using Breeze.Services.Logging;
using Breeze.Services.MemoryCache;
using Breeze.Services.OTP;
using Breeze.Services.Scheduled;
using Breeze.Services.Subject;
using Breeze.Utilities.HttpClientManager;
using Microsoft.EntityFrameworkCore;

namespace Breeze.API.Extensions;

public static class ApplicationServicesExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IHttpClientWrapper, HttpClientWrapper>();
        services.AddAutoMapper(typeof(AutoMappingProfile).Assembly);
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddHttpContextAccessor();
        services.AddScoped<IHttpHeaderService, HttpHeaderService>();

        services.AddScoped<IClaimResolverService, ClaimResolverService>();

        services.AddScoped<IEmailService, EmailService>();

        services.AddScoped<IDropDownService, DropDownService>();

        services.AddScoped<IAuthFacadeService, AuthFacadeService>();
        services.AddScoped<IAuthService, AuthService>();

        services.AddScoped<IOTPFacadeService, OTPFacadeService>();
        services.AddScoped<IOTPService, OTPService>();

        services.AddScoped<ILoggingService, LoggingService>();

        services.AddLazyCache();
        services.AddSingleton<IMemoryCacheService, MemoryCacheService>();

        services.AddSingleton<ICacheService, CacheService>();

        services.AddScoped<ISubjectFacadeService, SubjectFacadeService>();
        services.AddScoped<ISubjectService, SubjectService>();

        services.AddScoped<IIdentityService, IdentityService>();

        services.AddScoped<IScheduledService, ScheduledService>();

        //services.AddSingleton<IBlobStorageService>(provider =>
        // new BlobStorageService(configuration["BlobStorage:ConnectionString"]!));

        //services.AddScoped<ISecretManagerService>(provider =>
        //    new SecretManagerService(configuration["KeyVaultConfiguration:KeyVaultUrl"]!));

        //services.AddScoped<IBreezeDbContext, BreezeDbContext>();
        //services.AddDbContext<BreezeDbContext>((provider, options) =>
        //{
        //    var secretManagerService = new SecretManagerService(configuration["KeyVaultConfiguration:KeyVaultUrl"]!);
        //    var connectionString = secretManagerService.GetSecretAsync(configuration["KeyVaultConfiguration:SecretNames:ConnectionString"]!).GetAwaiter().GetResult();

        //    options.UseSqlServer(connectionString);
        //},
        //ServiceLifetime.Scoped,
        //ServiceLifetime.Singleton);


        services.AddScoped<IBreezeDbContext, BreezeDbContext>();
        services.AddDbContext<BreezeDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                sqlServerOptionsAction =>
                {
                    sqlServerOptionsAction.EnableRetryOnFailure(maxRetryCount: 3, maxRetryDelay: TimeSpan.FromSeconds(1), errorNumbersToAdd: Array.Empty<int>());
                    sqlServerOptionsAction.CommandTimeout(30);
                });
        },
        ServiceLifetime.Scoped,
        ServiceLifetime.Singleton);

        services.AddScoped<DeviceValidatorAttribute>();
        return services;
    }
}