namespace Breeze.API.Extensions;

public static class ApplicationConfigurationExtension
{
    public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseConfiguration>(configuration.GetSection("DatabaseConfiguration"));
        services.Configure<EmailConfiguration>(configuration.GetSection("EmailConfiguration"));
        services.Configure<AuthenticationConfiguration>(configuration.GetSection("AuthenticationConfiguration"));
        services.Configure<HashingConfiguration>(configuration.GetSection("HashingConfiguration"));
        return services;
    }
}