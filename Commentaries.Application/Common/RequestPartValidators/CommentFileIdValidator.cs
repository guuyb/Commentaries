using FluentValidation;
using Commentaries.Application.Common.RequestParts;

namespace Commentaries.Application.Common.RequestPartValidators;

internal class CommentFileIdValidator : AbstractValidator<IHasCommentFileId>
{
    public CommentFileIdValidator()
    {
        RuleFor(x => x.CommentFileId)
            .NotEmpty();
    }
}
