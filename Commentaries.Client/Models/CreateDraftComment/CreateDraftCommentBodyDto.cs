namespace Commentaries.Client.Models.CreateDraftComment;

public record CreateDraftCommentBodyDto(
    string AuthorId,
    string? Content,
    string ObjectId,
    string ObjectTypeFullName);
