using Commentaries.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Commentaries.Infrastructure.SecondaryAdapters.Db.Configurations;

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
