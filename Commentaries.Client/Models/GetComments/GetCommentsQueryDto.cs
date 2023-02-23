using Commentaries.Client.Models.Common;
using System;

namespace Commentaries.Client.Models.GetComments;

public record GetCommentsQueryDto(
    int PageNumber,
    int PageSize,
    int Order,
    string Member,
    string? ObjectId = null,
    string? ObjectTypeFullName = null,
    CommentStateEnum? StateId = null,
    Guid[]? CommentIds = null);
