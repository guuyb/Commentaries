using Commentaries.Data.Attributes;
using Commentaries.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace Commentaries.Data.Models;

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

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comment", "public");

        builder.HasKey(p => p.Id);
        builder.HasIndex(p => new { p.ObjectId, p.ObjectTypeId });

        builder.Property(p => p.AuthorId)
            .HasMaxLength(Comment.AUTHOR_ID_MAX_LENGTH)
            .IsRequired();

        builder.Property(p => p.CreatedDate)
            .HasUtcDateTimeConversion();

        builder.Property(p => p.UpdatedDate)
            .HasUtcDateTimeConversion();

        builder.Property(p => p.StateId)
            .HasConversion<int>();

        builder.Property(p => p.PublishedDate)
            .HasUtcDateTimeConversion();

        builder.Property(p => p.Content)
            .HasMaxLength(Comment.CONTENT_MAX_LENGTH);

        builder.Property(p => p.ObjectId)
            .HasMaxLength(Comment.OBJECT_ID_MAX_LENGTH)
            .IsRequired();

        builder
            .HasOne(p => p.ObjectType)
            .WithMany()
            .HasForeignKey(p => p.ObjectTypeId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(p => p.State)
            .WithMany()
            .HasForeignKey(p => p.StateId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
