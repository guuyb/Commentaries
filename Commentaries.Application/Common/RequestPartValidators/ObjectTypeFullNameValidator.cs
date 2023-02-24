using Commentaries.Domain.Models;
using Commentaries.Application.Common.RequestParts;
using FluentValidation;

namespace Commentaries.Application.Common.RequestPartValidators;

internal class ObjectTypeFullNameValidator : AbstractValidator<IHasObjectTypeFullName>
{
    public ObjectTypeFullNameValidator()
    {
        RuleFor(x => x.ObjectTypeFullName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(ObjectType.FULL_NAME_MAX_LENGTH)
            .MatchObjectTypeFullName();
    }
}
