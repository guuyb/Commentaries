using Commentaries.Application.Ports;
using Commentaries.Infrastructure.SecondaryAdapters.Db;
using Commentaries.Infrastructure.SecondaryAdapters.Db.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Commentaries.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        var postgreAssembly = typeof(CommentConfiguration).Assembly;
        services.AddTransient(sp => new DbConfigurationsOptions(postgreAssembly));
        services.AddDbContext<ICommentariesDbContext, CommentariesDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString(nameof(CommentariesDbContext)),
                b => b.MigrationsAssembly(postgreAssembly.FullName)));

        return services;
    }
}
