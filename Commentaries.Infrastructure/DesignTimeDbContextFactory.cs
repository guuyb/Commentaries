using Commentaries.Infrastructure.SecondaryAdapters.Db;
using Commentaries.Infrastructure.SecondaryAdapters.Db.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Commentaries.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<CommentariesDbContext>
{
    CommentariesDbContext IDesignTimeDbContextFactory<CommentariesDbContext>.CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json")
             .Build();

        var builder = new DbContextOptionsBuilder<CommentariesDbContext>();

        var connectionString = configuration.GetConnectionString(nameof(CommentariesDbContext));

        var postgreAssembly = typeof(CommentConfiguration).Assembly;
        builder.UseNpgsql(connectionString, o =>
        {
            o.MigrationsAssembly(postgreAssembly.FullName);
            //o.SetPostgresVersion(new Version("9.6"));
        });

        return new CommentariesDbContext(builder.Options,
            new DbConfigurationsOptions(postgreAssembly));
    }
}
