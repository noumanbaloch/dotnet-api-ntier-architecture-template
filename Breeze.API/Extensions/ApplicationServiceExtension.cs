using Breeze.API.Filter;
using Breeze.API.MappingProfile;
using Breeze.DbCore.Context;
using Breeze.DbCore.UnitOfWork;
using Breeze.Services.Auth;
using Breeze.Services.Cache;
using Breeze.Services.ClaimResolver;
using Breeze.Services.DropDown;
using Breeze.Services.Email;
using Breeze.Services.HttpHeader;
using Breeze.Services.Logging;
using Breeze.Services.MemoryCache;
using Breeze.Services.OTP;
using Breeze.Services.ParamBuilder;
using Breeze.Services.Subject;
using Breeze.Services.TokenService;
using Microsoft.EntityFrameworkCore;

namespace Breeze.API.Extensions;

public static class ApplicationServiceExtension
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(typeof(AutoMappingProfile).Assembly);
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<IHttpHeaderService, HttpHeaderService>();

        services.AddScoped<IClaimResolverService, ClaimResolverService>();

        services.AddScoped<IEmailService, EmailService>();

        services.AddScoped<ITokenService, TokenService>();

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

        services.AddScoped<IParamBuilderService, ParamBuilderService>();

        //string connectionString = configuration.GetSection("AzureBlobStorage:ConnectionString").Value!;
        //services.AddSingleton<IAzureBlobStorageService>(new AzureBlobStorageService(connectionString));


        //string keyVaultUrl = "https://yourserver-azurekeyvault.vault.azure.net/";
        //services.AddScoped<ISecretManagerService>(provider =>
        //    new SecretManagerService(keyVaultUrl));

        //services.AddScoped<IDatabaseContext, DatabaseContext>();
        //services.AddDbContext<DatabaseContext>((provider, options) =>
        //{
        //    var secretManagerService = provider.GetService<ISecretManagerService>();
        //    var connectionString = secretManagerService.GetSecretAsync("QA-DBConnectionString").GetAwaiter().GetResult();

        //    options.UseSqlServer(connectionString);
        //});2


        services.AddScoped<IDatabaseContext, DatabaseContext>();
        services.AddDbContext<DatabaseContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        services.AddScoped<DeviceValidatorAttribute>();
        return services;
    }
}