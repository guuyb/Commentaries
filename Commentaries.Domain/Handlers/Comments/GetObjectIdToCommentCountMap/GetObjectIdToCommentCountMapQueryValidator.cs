using Commentaries.Domain.Common.RequestParts;
using FluentValidation;
using System.Linq;

namespace Commentaries.Domain.Handlers.Comments.GetObjectIdToCommentCountMap;

internal class GetObjectIdToCommentCountMapQueryValidator
    : AbstractValidator<GetObjectIdToCommentCountMapQuery>
{
    public GetObjectIdToCommentCountMapQueryValidator(
        IValidator<IHasObjectTypeFullName> objectTypeFullNameValidator,
        IValidator<IHasObjectId> objectIdValidator)
    {
        RuleFor(x => x.ObjectIds).NotNull();
        RuleForEach(x => x.ObjectIds)
            .Custom((id, context) =>
            {
                objectIdValidator.Validate(new HasObjectId(id))
                    .Errors.ToList()
                    .ForEach(f => context.AddFailure(f));
            });

        Include(objectTypeFullNameValidator);
    }

    private record class HasObjectId(string ObjectId) : IHasObjectId;
}
