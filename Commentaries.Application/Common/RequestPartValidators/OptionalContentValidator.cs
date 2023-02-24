using FluentValidation;
using Commentaries.Domain.Models;
using Commentaries.Application.Common.RequestParts;

namespace Commentaries.Application.Common.RequestPartValidators;

internal class OptionalContentValidator : AbstractValidator<IHasOptionalContent>
{
    public OptionalContentValidator()
    {
        RuleFor(x => x.Content)
            .MaximumLength(Comment.CONTENT_MAX_LENGTH);
    }
}
