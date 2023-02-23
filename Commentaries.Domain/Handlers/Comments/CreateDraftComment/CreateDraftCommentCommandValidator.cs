using Commentaries.Domain.Common.RequestParts;
using FluentValidation;

namespace Commentaries.Domain.Handlers.Comments.CreateDraftComment;

internal class CreateDraftCommentCommandValidator : AbstractValidator<CreateDraftCommentCommand>
{
    public CreateDraftCommentCommandValidator(
        IValidator<IHasAuthorId> authorIdValidator,
        IValidator<IHasOptionalContent> contentValidator,
        IValidator<IHasObjectId> objectIdValidator,
        IValidator<IHasObjectTypeFullName> objectTypeFullNameValidator)
    {
        Include(authorIdValidator);
        Include(contentValidator);
        Include(objectIdValidator);
        Include(objectTypeFullNameValidator);
    }
}
