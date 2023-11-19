using Hangfire;
using HangfireBasicAuthenticationFilter;

namespace Breeze.API.Extensions;

public static class HangfireConfigurationExtension
{
    public static IServiceCollection AddHangfire(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddHangfire(config
            => config.SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));

        services.AddHangfireServer();
        return services;
    }

    public static IApplicationBuilder UseHangfire(this IApplicationBuilder app,
        IConfiguration configuration,
        IRecurringJobManager recurringJobManager,
        IServiceProvider serviceProvider)
    {

        app.UseHangfireDashboard("/hangfiredashboard", new DashboardOptions
        {
            DashboardTitle = "Hangfire Dashboard",
            AppPath = "https://localhost:44348/swagger",
            Authorization = new[] {
                new HangfireCustomBasicAuthenticationFilter
                {
                    User = configuration.GetSection("HangfireSettings:UserName").Value!,
                    Pass = configuration.GetSection("HangfireSettings:Password").Value!
                }
            }
        });


        return app;
    }
}