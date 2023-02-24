using FluentValidation;
using Commentaries.Application.Common.RequestParts;

namespace Commentaries.Application.Common.RequestPartValidators;

internal class AuthorIdValidator : AbstractValidator<IHasAuthorId>
{
    public AuthorIdValidator()
    {
        RuleFor(x => x.AuthorId)
            .NotEmpty();
    }
}
