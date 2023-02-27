using Commentaries.Domain.Attributes;
using System;

namespace Commentaries.Domain.Models;

[LocalizedName("Файл комментария")]
public class CommentFile
{
    public const int FILE_NAME_MAX_LENGTH = 250;

    public Guid Id { get; set; }
    public byte[] Data { get; set; } = new byte[0];
    public Guid CommentId { get; set; }
    public Comment? Comment { get; set; }
    public string FileName { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime DeletedAt { get; set; }
}
