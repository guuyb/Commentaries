using FluentValidation;
using Commentaries.Application.Common.RequestParts;

namespace Commentaries.Application.Common.RequestPartValidators;

internal class CommentIdValidator : AbstractValidator<IHasCommentId>
{
    public CommentIdValidator()
    {
        RuleFor(x => x.CommentId)
            .NotEmpty();
    }
}
