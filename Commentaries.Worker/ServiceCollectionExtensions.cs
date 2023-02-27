using Commentaries.Application.Common.Models;
using Commentaries.Infrastructure.SecondaryAdapters.Db;
using Commentaries.Infrastructure.SecondaryAdapters.Db.Models;
using Commentaries.Worker.Configs;
using Commentaries.Worker.Consumers;
using Commentaries.Worker.HostedServices;
using Commentaries.Worker.Models;
using Guuyb.Mq.Extensions;
using Guuyb.OutboxMessaging.Worker;

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
                r.Add<CommentCreatedMqDto, CommentariesIncomingQueueConsumer>();
                r.Add<EraseAbandonedCommentMqDto, CommentariesIncomingQueueConsumer>();
            });

        services.AddDbSetOutboxMessagingWorker<OutboxMessage, CommentariesDbContext>(
            configuration.GetSection(nameof(OutboxMessagingWorkerConfig)).Bind);

        return services;
    }
}
