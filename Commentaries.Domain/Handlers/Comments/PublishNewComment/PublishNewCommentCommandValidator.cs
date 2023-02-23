using FluentValidation;
using Commentaries.Domain.Common.RequestParts;

namespace Commentaries.Domain.Handlers.Comments.PublishNewComment;

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
