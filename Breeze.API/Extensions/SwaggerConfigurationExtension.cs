using Microsoft.OpenApi.Models;
using Sharkemm.API.Filter;

namespace Breeze.API.Extensions;

public static class SwaggerConfigurationExtension
{
    public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddSwaggerGen(c =>
        {

            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Breeze Core API", Version = "v1.0" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });


            c.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
            });
            if (env.IsDevelopment() || env.IsQA()) c.OperationFilter<AddHeaderParameterFilter>();
        });

        return services;
    }
}
