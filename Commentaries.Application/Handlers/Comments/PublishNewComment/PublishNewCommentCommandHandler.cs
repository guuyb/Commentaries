using Commentaries.Domain.Models;
using Commentaries.Application.Common.RequestParts;
using Commentaries.Application.Ports;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Commentaries.Application.Handlers.Comments.PublishNewComment;

public record PublishNewCommentCommand(
    string AuthorId,
    string Content,
    DateTime PublishedDate,
    string ObjectId,
    string ObjectTypeFullName
    ) : IRequest<PublishNewCommentResultDto>, IHasAuthorId, IHasObjectId, IHasObjectTypeFullName, IHasContent;

internal sealed class PublishNewCommentCommandHandler
    : IRequestHandler<PublishNewCommentCommand, PublishNewCommentResultDto>
{
    private readonly ICommentariesDbContext _context;
    private readonly IValidator<PublishNewCommentCommand> _commandValidator;

    public PublishNewCommentCommandHandler(ICommentariesDbContext context,
        IValidator<PublishNewCommentCommand> commandValidator)
    {
        _context = context;
        _commandValidator = commandValidator;
    }

    public async Task<PublishNewCommentResultDto> Handle(PublishNewCommentCommand command, CancellationToken cancellationToken)
    {
        await _commandValidator.ValidateOrThrowExceptionAsync(command, cancellationToken);

        var objectType = await _context.ObjectTypes
            .FirstOrDefaultAsync(t => t.FullName == command.ObjectTypeFullName, cancellationToken);

        if (objectType is null)
        {
            objectType = new ObjectType
            {
                FullName = command.ObjectTypeFullName,
                CreatedDate = DateTime.UtcNow,
            };
        }

        var comment = new Comment
        {
            AuthorId = command.AuthorId,
            CreatedDate = DateTime.UtcNow,
            StateId = CommentStateEnum.Published,
            PublishedDate = command.PublishedDate,
            Content = command.Content?.Trim(),
            ObjectId = command.ObjectId,
            ObjectType = objectType,
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync(cancellationToken);

        return new(comment.Id);
    }
}
