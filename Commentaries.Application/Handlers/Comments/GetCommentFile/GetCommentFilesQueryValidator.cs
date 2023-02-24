using Commentaries.Application.Common.RequestPartValidators;
using FluentValidation;

namespace Commentaries.Application.Handlers.Comments.GetCommentFile;

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
