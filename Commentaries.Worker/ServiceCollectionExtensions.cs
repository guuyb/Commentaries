using Commentaries.Infrastructure.SecondaryAdapters.Db;
using Commentaries.Worker.Configs;
using Commentaries.Worker.Consumers;
using Commentaries.Worker.HostedServices;
using Commentaries.Worker.Models;
using Guuyb.Mq.Extensions;

namespace Commentaries.Worker;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWorker(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddOptions();
        services.AddHostedService<DbUpdaterService<CommentariesDbContext>>();

        services.RegisterMq(configuration);
        services.BindConsumers<CommentariesConsumerBindingServiceConfig>(
            configuration, r =>
            {
                r.Add<PublishNewCommentMqDto, CommentariesIncomingQueueConsumer>();
            });

        return services;
    }
}
