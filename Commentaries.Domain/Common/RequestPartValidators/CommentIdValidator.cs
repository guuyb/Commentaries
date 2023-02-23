using FluentValidation;
using Commentaries.Domain.Common.RequestParts;

namespace Commentaries.Domain.Common.RequestPartValidators;

internal class CommentIdValidator : AbstractValidator<IHasCommentId>
{
    public CommentIdValidator()
    {
        RuleFor(x => x.CommentId)
            .NotEmpty();
    }
}
