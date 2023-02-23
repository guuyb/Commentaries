using Commentaries.Domain.Common.RequestParts;
using FluentValidation;

namespace Commentaries.Domain.Handlers.Comments.PublishComment;

internal class PublishCommentCommandValidator : AbstractValidator<PublishCommentCommand>
{
    public PublishCommentCommandValidator(
        IValidator<IHasCommentId> commentIdValidator)
    {
        Include(commentIdValidator);
    }
}
