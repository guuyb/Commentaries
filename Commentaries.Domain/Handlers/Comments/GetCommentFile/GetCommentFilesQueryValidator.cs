using Commentaries.Domain.Common.RequestPartValidators;
using FluentValidation;

namespace Commentaries.Domain.Handlers.Comments.GetCommentFile;

internal class GetCommentFilesQueryValidator : AbstractValidator<GetCommentFileQuery>
{
    public GetCommentFilesQueryValidator(
        CommentIdValidator commentIdValidator,
        CommentFileIdValidator commentFileIdValidator)
    {
        Include(commentIdValidator);
        Include(commentFileIdValidator);
    }
}
