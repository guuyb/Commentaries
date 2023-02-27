using Commentaries.Domain.Models;
using Commentaries.Application.Common.Extensions;
using Commentaries.Application.Common.RequestParts;
using Commentaries.Application.Ports;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Commentaries.Application.Handlers.Comments.AddCommentFile;

public record RemoveCommentFileCommand(
    Guid CommentId,
    Guid CommentFileId) : IRequest, IHasCommentId, IHasCommentFileId;

internal sealed class RemoveCommentFileCommandHandler : IRequestHandler<RemoveCommentFileCommand>
{
    private readonly ICommentariesDbContext _context;
    private readonly IValidator<RemoveCommentFileCommand> _commandValidator;

    public RemoveCommentFileCommandHandler(ICommentariesDbContext context,
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
        file.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellation);

        return Unit.Value;
    }
}
