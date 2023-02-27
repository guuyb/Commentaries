using Commentaries.Domain.Models;
using Commentaries.Application.Common.Extensions;
using Commentaries.Application.Common.RequestParts;
using Commentaries.Application.Ports;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Commentaries.Application.Common.Models;

namespace Commentaries.Application.Handlers.Comments.CreateDraftComment;

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
    private readonly ICommentariesDbContext _context;
    private readonly IValidator<CreateDraftCommentCommand> _commandValidator;

    public CreateDraftCommentCommandHandler(ICommentariesDbContext context,
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
            Id = Guid.NewGuid(),
            AuthorId = command.AuthorId,
            CreatedAt = DateTime.UtcNow,
            StateId = CommentStateEnum.Draft,
            Content = command.Content,
            ObjectId = command.ObjectId,
            ObjectType = objectType,
        };

        _context.Comments.Add(comment);
        _context.Publish(new CommentCreatedMqDto(comment.Id),
            o => o.RoutingKey = "CommentCreated");
        await _context.SaveChangesAsync(cancellationToken);

        return new(comment.Id);
    }
}
