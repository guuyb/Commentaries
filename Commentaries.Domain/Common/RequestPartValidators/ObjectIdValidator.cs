using FluentValidation;
using Commentaries.Data.Models;
using Commentaries.Domain.Common.RequestParts;

namespace Commentaries.Domain.Common.RequestPartValidators;

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
