using Commentaries.Domain.Models;
using Commentaries.Application.Common.Extensions;
using Commentaries.Application.Common.RequestParts;
using Commentaries.Application.Ports;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Commentaries.Application.Handlers.Comments.AddCommentFile;

public record AddCommentFileCommand(
    Guid CommentId,
    string FileName,
    Stream Data) : IRequest<AddCommentFileResultDto>, IHasCommentId;

internal sealed class AddCommentFileCommandHandler : IRequestHandler<AddCommentFileCommand, AddCommentFileResultDto>
{
    private readonly ICommentariesDbContext _context;
    private readonly IValidator<AddCommentFileCommand> _commandValidator;

    public AddCommentFileCommandHandler(ICommentariesDbContext context,
        IValidator<AddCommentFileCommand> commandValidator)
    {
        _context = context;
        _commandValidator = commandValidator;
    }

    public async Task<AddCommentFileResultDto> Handle(AddCommentFileCommand command, CancellationToken cancellation)
    {
        await _commandValidator.ValidateOrThrowExceptionAsync(command, cancellation);

        var commentOrNull = await _context.Comments
            .Where(c => c.StateId != CommentStateEnum.Deleted)
            .Where(c => c.Id == command.CommentId)
            .FirstOrDefaultAsync(cancellation);

        commentOrNull.ThrowNotFoundIfNull(command.CommentId);
        var comment = commentOrNull!;

        CommentFile commentFile;
        using (MemoryStream ms = new MemoryStream())
        {
            command.Data.CopyTo(ms);
            commentFile = new CommentFile
            {
                CommentId = command.CommentId,
                FileName = command.FileName,
                Data = ms.ToArray(),
                UploadTimestamp = DateTime.UtcNow,
            };
        }
        _context.CommentFiles.Add(commentFile);

        await _context.SaveChangesAsync(cancellation);

        return new(commentFile.Id);
    }
}
