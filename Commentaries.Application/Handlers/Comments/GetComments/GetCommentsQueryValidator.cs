using Commentaries.Domain.Models;
using FluentValidation;

namespace Commentaries.Application.Handlers.Comments.GetComments;

internal class GetCommentsQueryValidator : AbstractValidator<GetCommentsQuery>
{
    public GetCommentsQueryValidator()
    {
        RuleFor(q => q.PageNumber)
            .GreaterThanOrEqualTo(1);
        RuleFor(q => q.PageSize)
            .InclusiveBetween(1, 200);
        RuleFor(q => q.Member)
            .NotEmpty();
        RuleFor(q => q.StateId)
            .IsInEnum();
        RuleFor(q => q.ObjectId)
            .MaximumLength(Comment.OBJECT_ID_MAX_LENGTH);
        RuleFor(q => q.ObjectTypeFullName)
            .MaximumLength(ObjectType.FULL_NAME_MAX_LENGTH)
            .MatchObjectTypeFullName();
    }
}
