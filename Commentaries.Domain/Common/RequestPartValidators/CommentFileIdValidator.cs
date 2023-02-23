using FluentValidation;
using Commentaries.Domain.Common.RequestParts;

namespace Commentaries.Domain.Common.RequestPartValidators;

internal class CommentFileIdValidator : AbstractValidator<IHasCommentFileId>
{
    public CommentFileIdValidator()
    {
        RuleFor(x => x.CommentFileId)
            .NotEmpty();
    }
}
