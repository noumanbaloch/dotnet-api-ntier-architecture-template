using Breeze.API.Extensions;
using Breeze.API.Middlewares;

namespace Breeze.API;

public class Startup
{
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment env;
    public Startup(IConfiguration configuration,
        IWebHostEnvironment env)
    {
        _configuration = configuration;
        this.env = env;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddApplicationService(_configuration)
            .AddSwaggerConfiguration(env)
            .AddApplicationConfiguration(_configuration)
            .AddIdentityService(_configuration)
            .AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
        });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment() || env.IsQA())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv7 v1"));
        }

        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseCors("AllowAllOrigins");
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
