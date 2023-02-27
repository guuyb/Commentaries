using Commentaries.Domain.Models;
using Commentaries.Application.Common.RequestParts;
using Commentaries.Application.Ports;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Commentaries.Application.Handlers.Comments.PublishComment;

public record PublishCommentCommand(Guid CommentId) : IRequest, IHasCommentId;

internal class PublishCommentCommandHandler : IRequestHandler<PublishCommentCommand>
{
    private readonly ICommentariesDbContext _context;
    private readonly IValidator<PublishCommentCommand> _publishCommentCommandValidator;

    public PublishCommentCommandHandler(ICommentariesDbContext context,
        IValidator<PublishCommentCommand> publishCommentCommandValidator)
    {
        _context = context;
        _publishCommentCommandValidator = publishCommentCommandValidator;
    }

    public async Task<Unit> Handle(PublishCommentCommand command, CancellationToken cancellation)
    {
        await _publishCommentCommandValidator.ValidateOrThrowExceptionAsync(command, cancellation);

        var commentOrNull = await _context.Comments
            .Where(c => c.StateId != CommentStateEnum.Deleted)
            .Where(c => c.Id == command.CommentId)
            .FirstOrDefaultAsync(cancellation);

        commentOrNull.ThrowNotFoundIfNull(command.CommentId);
        var comment = commentOrNull!;

        if (comment.StateId == CommentStateEnum.Published)
            return Unit.Value;

        if (comment.StateId != CommentStateEnum.Draft)
        {
            throw new Common.Exceptions.ValidationException(
                "Опубликовать комментарий можно только из статуса Черновик",
                "Wrong state of comment ({StateId})",
                comment.StateId);
        }

        if (string.IsNullOrWhiteSpace(comment.Content))
        {
            throw new Common.Exceptions.ValidationException(
                "Опубликовать комментарий можно только c заполненным содержанием",
                "Empty comment content");
        }

        comment.StateId = CommentStateEnum.Published;
        comment.PublishedAt = DateTime.UtcNow;
        comment.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellation);

        return Unit.Value;
    }
}
