using Commentaries.Data.Models;
using Commentaries.Domain.Common.RequestParts;
using FluentValidation;

namespace Commentaries.Domain.Common.RequestPartValidators;

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
