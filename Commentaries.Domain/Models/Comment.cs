using Commentaries.Domain.Attributes;
using System;
using System.Collections.Generic;

namespace Commentaries.Domain.Models;

[LocalizedName("Комментарий")]
public class Comment
{
    public const int AUTHOR_ID_MAX_LENGTH = 50;
    public const int CONTENT_MAX_LENGTH = 1000;
    public const int OBJECT_ID_MAX_LENGTH = 50;

    public Guid Id { get; set; }

    /// <summary>
    /// ИД автора комментария
    /// </summary>
    public string AuthorId { get; set; } = string.Empty;

    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedDate { get; set; }

    public DateTime UpdatedDate { get; set; }

    /// <summary>
    /// Статус
    /// </summary>
    public CommentStateEnum StateId { get; set; }
    public CommentState? State { get; set; }

    /// <summary>
    /// Дата публикации
    /// </summary>
    public DateTime? PublishedDate { get; set; }

    /// <summary>
    /// Содержание
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// ИД комментируемого объекта
    /// </summary>
    public string ObjectId { get; set; } = string.Empty;

    /// <summary>
    /// ИД типа комментируемого объекта
    /// </summary>
    public int ObjectTypeId { get; set; }
    public ObjectType? ObjectType { get; set; }

    public ICollection<CommentFile> Files { get; set; } = new List<CommentFile>();
}
