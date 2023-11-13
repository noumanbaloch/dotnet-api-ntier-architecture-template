using Breeze.API;
using Breeze.Models.Constants;
using Serilog;

namespace Breeze.API;
public class Program
{
    public static void Main(string[] args)
    {
        //var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        //var configuration = new ConfigurationBuilder()
        //    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //    .AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true)
        //    .AddEnvironmentVariables()
        //    .Build();

        //var connectionString = configuration["AzureBlobStorage:ConnectionString"];
        //var containerName = ContainerNames.APPLICATION_LOGS_CONTAINER;

        //Log.Logger = new LoggerConfiguration()
        //    .WriteTo.AzureBlobStorage(
        //        connectionString: connectionString,
        //        restrictedToMinimumLevel: LogEventLevel.Warning,
        //        storageContainerName: containerName,
        //        storageFileName: "breeze-application-logs.txt",
        //        writeInBatches: true,
        //        period: TimeSpan.FromSeconds(2),
        //        batchPostingLimit: 100,
        //        bypassBlobCreationValidation: false,
        //        cloudBlobProvider: null,
        //        blobSizeLimitBytes: 5000000,
        //        retainedBlobCountLimit: null,
        //        useUtcTimeZone: true)
        //    .CreateLogger();

        try
        {
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, ExceptionMessages.FAILED_TO_START_API);
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}