using Commentaries.Client.Models.Common;
using System;

namespace Commentaries.Client.Models.GetComment;

public record GetCommentResultDto(CommentDto Comment);

public record CommentDto(
    Guid Id,
    string AuthorId,
    string? Content,
    CommentStateEnum StateId,
    DateTime CreatedDate,
    DateTime? PublishedDate,
    FileDto[] Files);

public record FileDto(Guid Id, string FileName);
