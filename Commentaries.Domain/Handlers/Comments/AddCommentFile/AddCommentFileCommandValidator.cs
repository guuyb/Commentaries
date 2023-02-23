using Commentaries.Data.Models;
using Commentaries.Domain.Common.RequestPartValidators;
using FluentValidation;

namespace Commentaries.Domain.Handlers.Comments.AddCommentFile;

internal class AddCommentFileCommandValidator : AbstractValidator<AddCommentFileCommand>
{
    public AddCommentFileCommandValidator(
        CommentIdValidator commentIdValidator)
    {
        RuleFor(c => c.FileName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MaximumLength(CommentFile.FILE_NAME_MAX_LENGTH);

        RuleFor(c => c.Data)
            .NotEmpty();

        Include(commentIdValidator);
    }
}
