using FluentValidation;
using Commentaries.Data.Models;
using Commentaries.Domain.Common.RequestParts;

namespace Commentaries.Domain.Common.RequestPartValidators;

internal class OptionalContentValidator : AbstractValidator<IHasOptionalContent>
{
    public OptionalContentValidator()
    {
        RuleFor(x => x.Content)
            .MaximumLength(Comment.CONTENT_MAX_LENGTH);
    }
}
