using Commentaries.Domain.Common.RequestParts;
using FluentValidation;

namespace Commentaries.Domain.Handlers.Comments.EditCommentContent;

internal class EditCommentContentCommandValidator : AbstractValidator<EditCommentContentCommand>
{
    public EditCommentContentCommandValidator(
        IValidator<IHasCommentId> commentIdValidator,
        IValidator<IHasOptionalContent> optionalContentValidator)
    {
        Include(commentIdValidator);
        Include(optionalContentValidator);
    }
}
