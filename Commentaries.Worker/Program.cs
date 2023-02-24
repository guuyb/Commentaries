using Commentaries.Application;
using Commentaries.Infrastructure;
using Serilog;

namespace Commentaries.Worker;

public class Program
{
    public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
        .AddEnvironmentVariables()
        .Build();

    public static int Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .CreateLogger();
        int exitCode = 0;
        try
        {
            Log.Information("Starting service");
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Service terminated unexpectedly");
            exitCode = 1;
        }
        finally
        {
            Log.CloseAndFlush();
            Environment.Exit(exitCode);
        }

        return 0;
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .UseWindowsService()
            .ConfigureServices((hostContext, services) =>
            {
                services.AddApplication(Configuration);
                services.AddInfrastructure(Configuration);
                services.AddWorker(Configuration);
            });
    }
}
