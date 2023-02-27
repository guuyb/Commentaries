using Commentaries.Application.Common.RequestParts;
using Commentaries.Application.Handlers.Comments.EraseComment;
using FluentValidation;

namespace Commentaries.Application.Handlers.Comments.EraseAbandonedComment;

internal class EraseAbandonedCommentCommandValidator : AbstractValidator<EraseAbandonedCommentCommand>
{
    public EraseAbandonedCommentCommandValidator(
        IValidator<IHasCommentId> commentIdValidator)
    {
        Include(commentIdValidator);
    }
}
