using Commentaries.Application.Common.Models;
using Commentaries.Application.Handlers.Comments.EraseComment;
using Commentaries.Application.Handlers.Comments.PublishNewComment;
using Commentaries.Infrastructure.SecondaryAdapters.Db;
using Commentaries.Worker.Configs;
using Commentaries.Worker.Models;
using Guuyb.Mq;
using MediatR;
using Microsoft.Extensions.Options;

namespace Commentaries.Worker.Consumers;

public class CommentariesIncomingQueueConsumer :
    IMessageConsumer<PublishNewCommentMqDto>,
    IMessageConsumer<CommentCreatedMqDto>,
    IMessageConsumer<EraseAbandonedCommentMqDto>
{
    private readonly IMediator _mediator;
    private readonly CommentariesDbContext _dbContext;
    private readonly CommentariesConsumerBindingServiceConfig _commentariesConsumerBindingServiceConfig;

    public CommentariesIncomingQueueConsumer(IMediator mediator,
        CommentariesDbContext dbContext,
        IOptions<CommentariesConsumerBindingServiceConfig> commentariesConsumerBindingServiceConfig)
    {
        _mediator = mediator;
        _dbContext = dbContext;
        _commentariesConsumerBindingServiceConfig = commentariesConsumerBindingServiceConfig.Value;
    }

    public Task ConsumeAsync(PublishNewCommentMqDto message, CancellationToken cancellationToken)
    {
        var command = new PublishNewCommentCommand(
            message.AuthorId,
            message.Content,
            message.PublishedDate,
            message.ObjectId,
            message.ObjectTypeFullName);
        return _mediator.Send(command, cancellationToken);
    }

    public Task ConsumeAsync(CommentCreatedMqDto message, CancellationToken cancellationToken)
    {
        _dbContext.Send(new EraseAbandonedCommentMqDto(message.CommentId),
            _commentariesConsumerBindingServiceConfig.QueueName,
            o => o.DelayUntil = DateTime.UtcNow.AddDays(7)); // todo: transfer 7 to config
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task ConsumeAsync(EraseAbandonedCommentMqDto message, CancellationToken cancellationToken)
    {
        return _mediator.Send(new EraseAbandonedCommentCommand(message.CommentId), cancellationToken);
    }
}