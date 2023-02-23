using Commentaries.Data;
using Commentaries.Data.Models;
using Commentaries.Domain.Common.Extensions;
using Commentaries.Domain.Common.RequestParts;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Commentaries.Domain.Handlers.Comments.CreateDraftComment;

public record CreateDraftCommentCommand(
    string AuthorId,
    string? Content,
    string ObjectId,
    string ObjectTypeFullName) : IRequest<CreateDraftCommentResultDto>,
    IHasAuthorId, IHasObjectId,
    IHasObjectTypeFullName, IHasOptionalContent;

internal sealed class CreateDraftCommentCommandHandler
    : IRequestHandler<CreateDraftCommentCommand, CreateDraftCommentResultDto>
{
    private readonly CommentariesContext _context;
    private readonly IValidator<CreateDraftCommentCommand> _commandValidator;

    public CreateDraftCommentCommandHandler(CommentariesContext context,
        IValidator<CreateDraftCommentCommand> commandValidator)
    {
        _context = context;
        _commandValidator = commandValidator;
    }

    public async Task<CreateDraftCommentResultDto> Handle(CreateDraftCommentCommand command, CancellationToken cancellationToken)
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
            StateId = CommentStateEnum.Draft,
            Content = command.Content,
            ObjectId = command.ObjectId,
            ObjectType = objectType,
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync(cancellationToken);

        return new(comment.Id);
    }
}
