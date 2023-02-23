using Commentaries.Domain.Common.RequestPartValidators;
using FluentValidation;

namespace Commentaries.Domain.Handlers.Comments.AddCommentFile;

internal class RemoveCommentFileCommandValidator : AbstractValidator<RemoveCommentFileCommand>
{
    public RemoveCommentFileCommandValidator(
        CommentIdValidator commentIdValidator,
        CommentFileIdValidator commentFileIdValidator)
    {
        Include(commentIdValidator);
        Include(commentFileIdValidator);
    }
}
