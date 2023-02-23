using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Commentaries.Data.Test;

public class Startup
{
    public static IConfiguration Configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json")
        .Build();

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<CommentariesContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("CommentariesContext")));
    }
}
