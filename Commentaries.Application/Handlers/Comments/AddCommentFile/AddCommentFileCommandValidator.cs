using Commentaries.Domain.Models;
using Commentaries.Application.Common.RequestPartValidators;
using FluentValidation;

namespace Commentaries.Application.Handlers.Comments.AddCommentFile;

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
