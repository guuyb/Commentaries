using Commentaries.Domain.Models;
using Commentaries.Application.Common;
using Commentaries.Application.Common.RequestParts;
using Commentaries.Application.Ports;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Commentaries.Application.Handlers.Comments.EditCommentContent;

public record class EditCommentContentCommand(Guid CommentId, string? Content)
    : IRequest, IHasCommentId, IHasOptionalContent;

internal class EditCommentContentCommandHandler : IRequestHandler<EditCommentContentCommand>
{
    private readonly ICommentariesDbContext _context;
    private readonly IValidator<EditCommentContentCommand> _commandValidator;

    public EditCommentContentCommandHandler(ICommentariesDbContext context,
        IValidator<EditCommentContentCommand> commandValidator)
    {
        _context = context;
        _commandValidator = commandValidator;
    }

    public async Task<Unit> Handle(EditCommentContentCommand command, CancellationToken cancellation)
    {
        await _commandValidator.ValidateOrThrowExceptionAsync(command, cancellation);

        var commentOrNull = await _context.Comments
            .Where(c => c.StateId != CommentStateEnum.Deleted)
            .Where(c => c.Id == command.CommentId)
            .FirstOrDefaultAsync(cancellation);

        commentOrNull.ThrowNotFoundIfNull(command.CommentId);
        var comment = commentOrNull!;

        if (comment.StateId == CommentStateEnum.Published
            && string.IsNullOrWhiteSpace(comment.Content))
        {
            throw new Common.Exceptions.ValidationException(
                "Требуется заполненное содержанием к опубликованному комментарию",
                "Empty comment content");
        }

        comment.Content = command.Content?.Trim();
        comment.UpdatedDate = DateTime.UtcNow;
        await _context.SaveChangesAsync(cancellation);

        return Unit.Value;
    }
}
