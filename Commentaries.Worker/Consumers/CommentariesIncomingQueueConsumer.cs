using Commentaries.Worker.Models;
using Guuyb.Mq;
using MediatR;

namespace Commentaries.Worker.Consumers;

public class CommentariesIncomingQueueConsumer :
    IMessageConsumer<PublishNewCommentMqDto>
{
    private readonly IMediator _mediator;

    public CommentariesIncomingQueueConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task ConsumeAsync(PublishNewCommentMqDto message, CancellationToken cancellationToken)
    {
        return _mediator.Send(message.CreatePublishNewCommentCommand(), cancellationToken);
    }
}