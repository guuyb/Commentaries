using Commentaries.Data;
using Commentaries.Data.Models;
using Commentaries.Domain.Common.Extensions;
using Commentaries.Domain.Common.RequestParts;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Commentaries.Domain.Handlers.Comments.AddCommentFile;

public record RemoveCommentFileCommand(
    Guid CommentId,
    Guid CommentFileId) : IRequest, IHasCommentId, IHasCommentFileId;

internal sealed class RemoveCommentFileCommandHandler : IRequestHandler<RemoveCommentFileCommand>
{
    private readonly CommentariesContext _context;
    private readonly IValidator<RemoveCommentFileCommand> _commandValidator;

    public RemoveCommentFileCommandHandler(CommentariesContext context,
        IValidator<RemoveCommentFileCommand> commandValidator)
    {
        _context = context;
        _commandValidator = commandValidator;
    }

    public async Task<Unit> Handle(RemoveCommentFileCommand command, CancellationToken cancellation)
    {
        await _commandValidator.ValidateOrThrowExceptionAsync(command, cancellation);

        var fileOrNull = await _context.CommentFiles
            .Where(f => f.Id == command.CommentFileId && !f.IsDeleted)
            .Where(f => f.Comment != null
                && f.Comment.Id == command.CommentId
                && f.Comment.StateId != CommentStateEnum.Deleted)
            .FirstOrDefaultAsync(cancellation);

        fileOrNull.ThrowNotFoundIfNull(command.CommentFileId);
        var file = fileOrNull!;

        file.IsDeleted = true;
        file.DeletedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}
