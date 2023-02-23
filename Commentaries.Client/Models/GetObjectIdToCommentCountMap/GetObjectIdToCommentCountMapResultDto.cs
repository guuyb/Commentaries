using System.Collections.Generic;

namespace Commentaries.Client.Models.GetObjectIdToCommentCountMap;

public record GetObjectIdToCommentCountMapResultDto(
    Dictionary<string, int> ObjectIdToCommentCountMap);
