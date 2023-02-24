using Commentaries.Application.Common.RequestParts;
using FluentValidation;

namespace Commentaries.Application.Handlers.Comments.PublishComment;

internal class PublishCommentCommandValidator : AbstractValidator<PublishCommentCommand>
{
    public PublishCommentCommandValidator(
        IValidator<IHasCommentId> commentIdValidator)
    {
        Include(commentIdValidator);
    }
}
