using Commentaries.Data;
using Commentaries.Data.Migrations.PostgreSql;
using Commentaries.Worker.Configs;
using Commentaries.Worker.Consumers;
using Commentaries.Worker.HostedServices;
using Commentaries.Worker.Models;
using Guuyb.Mq.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Commentaries.Worker;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWorker(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions();

        services.AddDbContext<CommentariesContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("CommentariesContext"),
            b => b.MigrationsAssembly(typeof(PostgreSqlDesignTimeDbContextFactory).Assembly.FullName)));

        services.AddHostedService<DbUpdaterService<CommentariesContext>>();

        services.RegisterMq(configuration);
        services.BindConsumers<CommentariesConsumerBindingServiceConfig>(
            configuration, r =>
            {
                r.Add<PublishNewCommentMqDto, CommentariesIncomingQueueConsumer>();
            });

        return services;
    }
}
