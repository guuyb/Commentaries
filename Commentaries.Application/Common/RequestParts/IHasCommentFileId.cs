using System;

namespace Commentaries.Application.Common.RequestParts;

public interface IHasCommentFileId
{
    Guid CommentFileId { get; }
}
