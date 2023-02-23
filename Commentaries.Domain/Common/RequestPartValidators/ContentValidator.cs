using FluentValidation;
using Commentaries.Data.Models;
using Commentaries.Domain.Common.RequestParts;

namespace Commentaries.Domain.Common.RequestPartValidators;

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
