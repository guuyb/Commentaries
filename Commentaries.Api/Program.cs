using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Commentaries.Api;

public class Program
{
    public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production"}.json",
            optional: true)
        .AddEnvironmentVariables()
        .Build();

    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(Configuration)
            .CreateLogger();
        try
        {
            Log.Information("Starting Commentaries.Api");
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Commentaries.Api terminated unexpectedly");
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
                webBuilder.ConfigureKestrel(o => o.Limits.MaxRequestBodySize = 50 * 1024 * 1024);
                webBuilder.UseStartup<Startup>();
            });
}
