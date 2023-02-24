using Commentaries.Application.Common.RequestPartValidators;
using FluentValidation;

namespace Commentaries.Application.Handlers.Comments.AddCommentFile;

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
