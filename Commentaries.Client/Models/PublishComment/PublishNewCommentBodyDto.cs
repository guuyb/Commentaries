using System;

namespace Commentaries.Client.Models.PublishComment;

public record PublishNewCommentBodyDto(
    string AuthorId,
    string Content,
    DateTime PublishedDate,
    string ObjectId,
    string ObjectTypeFullName);
