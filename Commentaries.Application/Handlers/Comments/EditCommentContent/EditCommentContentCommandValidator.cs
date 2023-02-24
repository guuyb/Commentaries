using Commentaries.Application.Common.RequestParts;
using FluentValidation;

namespace Commentaries.Application.Handlers.Comments.EditCommentContent;

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
