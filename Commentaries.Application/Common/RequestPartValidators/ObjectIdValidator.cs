using FluentValidation;
using Commentaries.Application.Common.RequestParts;
using Commentaries.Domain.Models;

namespace Commentaries.Application.Common.RequestPartValidators;

internal class ObjectIdValidator : AbstractValidator<IHasObjectId>
{
    public ObjectIdValidator()
    {
        RuleFor(x => x.ObjectId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(Comment.OBJECT_ID_MAX_LENGTH);
    }
}
