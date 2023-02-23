using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Commentaries.Data.Migrations.PostgreSql;

public class PostgreSqlDesignTimeDbContextFactory : IDesignTimeDbContextFactory<CommentariesContext>
{
    CommentariesContext IDesignTimeDbContextFactory<CommentariesContext>.CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.Migrations.PostgreSql.json")
             .Build();

        var builder = new DbContextOptionsBuilder<CommentariesContext>();

        var connectionString = configuration.GetConnectionString(nameof(CommentariesContext));

        builder.UseNpgsql(connectionString, o =>
        {
            o.MigrationsAssembly(GetType().Assembly.FullName);
            o.SetPostgresVersion(new Version("9.6"));
        });

        return new CommentariesContext(builder.Options);
    }
}
