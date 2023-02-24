using Commentaries.Application.Common.RequestParts;
using FluentValidation;

namespace Commentaries.Application.Handlers.Comments.CreateDraftComment;

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
