namespace Commentaries.Domain.Models;

public class CommentState
{
    public const int CODE_MAX_LENGTH = 50;

    public CommentStateEnum Id { get; set; }
    public string Code { get; set; } = string.Empty;
}
