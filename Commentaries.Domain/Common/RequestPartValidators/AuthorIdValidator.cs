using FluentValidation;
using Commentaries.Domain.Common.RequestParts;

namespace Commentaries.Domain.Common.RequestPartValidators;

internal class AuthorIdValidator : AbstractValidator<IHasAuthorId>
{
    public AuthorIdValidator()
    {
        RuleFor(x => x.AuthorId)
            .NotEmpty();
    }
}
