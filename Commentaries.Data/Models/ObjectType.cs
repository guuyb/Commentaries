using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Commentaries.Data.Models;

public class ObjectType
{
    public const int FULL_NAME_MAX_LENGTH = 200;

    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
}

public class ObjectTypeConfiguration : IEntityTypeConfiguration<ObjectType>
{
    public void Configure(EntityTypeBuilder<ObjectType> builder)
    {
        builder.ToTable("ObjectType", "public");

        builder.HasKey(p => p.Id);
        builder.HasIndex(p => p.FullName)
            .IsUnique();

        builder.Property(p => p.FullName)
            .HasMaxLength(ObjectType.FULL_NAME_MAX_LENGTH)
            .IsRequired();

        builder.Property(p => p.CreatedDate)
            .HasUtcDateTimeConversion();
    }
}
