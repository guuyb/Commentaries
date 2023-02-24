using FluentValidation;
using Commentaries.Domain.Models;
using Commentaries.Application.Common.RequestParts;

namespace Commentaries.Application.Common.RequestPartValidators;

internal class ContentValidator : AbstractValidator<IHasContent>
{
    public ContentValidator()
    {
        RuleFor(x => x.Content)
            .Cascade(CascadeMode.Stop)
            .Must(content => !string.IsNullOrWhiteSpace(content))
            .MaximumLength(Comment.CONTENT_MAX_LENGTH);
    }
}
