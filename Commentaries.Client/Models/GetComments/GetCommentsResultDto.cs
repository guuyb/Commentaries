using Commentaries.Client.Models.Common;
using System;
using System.Collections.Generic;

namespace Commentaries.Client.Models.GetComments;

public record GetCommentsResultDto(List<CommentDto> Comments, long TotalCount);

public record CommentDto(
    Guid Id,
    string AuthorId,
    string? Content,
    CommentStateEnum StateId,
    DateTime CreatedDate,
    DateTime? PublishedDate,
    FileDto[] Files);

public record FileDto(Guid Id, string FileName);
