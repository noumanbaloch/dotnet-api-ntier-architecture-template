using Breeze.API.Extensions;
using Breeze.API.Middlewares;
using Hangfire;

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
            .AddApplicationServices(_configuration)
            .AddSwaggerConfiguration(env)
            .AddApplicationConfiguration(_configuration)
            .AddIdentityService(_configuration)
            //.AddHangfire(_configuration) if you want to add hang fire uncomment this
            .AddControllers();

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
    public void Configure(IApplicationBuilder app,
        IWebHostEnvironment env
        //IRecurringJobManager recurringJobManager,
        //IServiceProvider serviceProvider
        )
    {
        if (env.IsDevelopment() || env.IsQA())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebAPIv8 v1"));
        }

        //app.UseHangfire(env, _configuration, recurringJobManager, serviceProvider); if you want to add hang fire uncomment this.

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
