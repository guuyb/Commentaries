using FluentValidation;
using Commentaries.Application.Common.RequestParts;
using Commentaries.Domain.Models;

namespace Commentaries.Application.Common.RequestPartValidators;

internal class OptionalContentValidator : AbstractValidator<IHasOptionalContent>
{
    public OptionalContentValidator()
    {
        RuleFor(x => x.Content)
            .MaximumLength(Comment.CONTENT_MAX_LENGTH);
    }
}
