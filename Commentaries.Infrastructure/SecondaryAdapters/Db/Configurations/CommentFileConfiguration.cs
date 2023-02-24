using Commentaries.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Commentaries.Infrastructure.SecondaryAdapters.Db.Configurations;

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
