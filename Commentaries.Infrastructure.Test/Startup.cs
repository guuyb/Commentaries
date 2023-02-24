using Commentaries.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Commentaries.Infrastructure.Test;

public class Startup
{
    public static IConfiguration Configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddInfrastructure(Configuration);
    }
}
