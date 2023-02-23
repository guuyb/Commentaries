using System.Collections.Generic;

namespace Commentaries.Domain.Handlers.Comments.GetObjectIdToCommentCountMap;

public record GetObjectIdToCommentCountMapResultDto(
    Dictionary<string, int> ObjectIdToCommentCountMap);
