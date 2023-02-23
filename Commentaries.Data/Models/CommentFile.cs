using Commentaries.Data.Attributes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Commentaries.Data.Models;

[LocalizedName("Файл комментария")]
public class CommentFile
{
    public const int FILE_NAME_MAX_LENGTH = 250;

    public Guid Id { get; set; }
    public byte[] Data { get; set; } = new byte[0];
    public Guid CommentId { get; set; }
    public Comment? Comment { get; set; }
    public string FileName { get; set; } = string.Empty;
    public DateTime UploadTimestamp { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime DeletedDate { get; set; }
}

public class CommentFileConfiguration : IEntityTypeConfiguration<CommentFile>
{
    public void Configure(EntityTypeBuilder<CommentFile> builder)
    {
        builder.ToTable("CommentFile", "public");

        builder.Property(prop => prop.FileName)
            .HasMaxLength(CommentFile.FILE_NAME_MAX_LENGTH);

        builder.Property(prop => prop.UploadTimestamp)
            .HasUtcDateTimeConversion();

        builder.HasOne(p => p.Comment)
            .WithMany(p => p.Files)
            .HasForeignKey(x => x.CommentId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Property(prop => prop.DeletedDate)
            .HasUtcDateTimeConversion();
    }
}
