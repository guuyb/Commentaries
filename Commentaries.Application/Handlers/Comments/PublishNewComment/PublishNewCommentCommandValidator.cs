using FluentValidation;
using Commentaries.Application.Common.RequestParts;

namespace Commentaries.Application.Handlers.Comments.PublishNewComment;

internal class PublishNewCommentCommandValidator : AbstractValidator<PublishNewCommentCommand>
{
    public PublishNewCommentCommandValidator(
        IValidator<IHasAuthorId> authorIdValidator,
        IValidator<IHasContent> contentValidator,
        IValidator<IHasObjectId> objectIdValidator,
        IValidator<IHasObjectTypeFullName> objectTypeFullNameValidator)
    {
        Include(authorIdValidator);
        Include(contentValidator);
        Include(objectIdValidator);
        Include(objectTypeFullNameValidator);
    }
}
