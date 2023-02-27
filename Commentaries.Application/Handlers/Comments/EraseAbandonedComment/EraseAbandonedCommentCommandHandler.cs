using Commentaries.Application.Common.Extensions;
using Commentaries.Application.Common.RequestParts;
using Commentaries.Application.Handlers.Comments.PublishComment;
using Commentaries.Application.Ports;
using Commentaries.Domain.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Commentaries.Application.Handlers.Comments.EraseComment;

public record EraseAbandonedCommentCommand(
    Guid CommentId) : IRequest, IHasCommentId;

internal sealed class EraseAbandonedCommentCommandHandler : IRequestHandler<EraseAbandonedCommentCommand>
{
    private readonly ICommentariesDbContext _context;
    private readonly IValidator<EraseAbandonedCommentCommand> _commandValidator;

    public EraseAbandonedCommentCommandHandler(ICommentariesDbContext context,
        IValidator<EraseAbandonedCommentCommand> commandValidator)
    {
        _context = context;
        _commandValidator = commandValidator;
    }

    public async Task<Unit> Handle(EraseAbandonedCommentCommand command, CancellationToken cancellation)
    {
        await _commandValidator.ValidateOrThrowExceptionAsync(command, cancellation);

        var commentOrNull = await _context.Comments
            .Include(c => c.Files)
            .Where(f => f.Id == command.CommentId)
            .FirstOrDefaultAsync(cancellation);

        commentOrNull.ThrowNotFoundIfNull(command.CommentId);
        var comment = commentOrNull!;

        if (!Comment.AbandonedRule.Compile().Invoke(comment))
            return Unit.Value;

        _context.Comments.Remove(comment);
        _context.CommentFiles.RemoveRange(comment.Files);
        await _context.SaveChangesAsync(cancellation);

        return Unit.Value;
    }
}
