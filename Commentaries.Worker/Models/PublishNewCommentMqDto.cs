using Commentaries.Application.Handlers.Comments.PublishNewComment;

namespace Commentaries.Worker.Models;

public record PublishNewCommentMqDto
{
    public string AuthorId { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PublishedDate { get; set; }
    public string ObjectId { get; set; } = string.Empty;
    public string ObjectTypeFullName { get; set; } = string.Empty;

    internal PublishNewCommentCommand CreatePublishNewCommentCommand()
    {
        return new(AuthorId, Content, PublishedDate, ObjectId, ObjectTypeFullName);
    }
}
