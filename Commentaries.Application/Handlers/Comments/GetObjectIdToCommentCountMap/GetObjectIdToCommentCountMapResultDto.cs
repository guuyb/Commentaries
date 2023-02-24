using System.Collections.Generic;

namespace Commentaries.Application.Handlers.Comments.GetObjectIdToCommentCountMap;

public record GetObjectIdToCommentCountMapResultDto(
    Dictionary<string, int> ObjectIdToCommentCountMap);
