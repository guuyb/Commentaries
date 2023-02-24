﻿// <auto-generated />
using System;
using Commentaries.Infrastructure.SecondaryAdapters.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Commentaries.Infrastructure.SecondaryAdapters.Db.Migrations
{
    [DbContext(typeof(CommentariesDbContext))]
    [Migration("20230224141832_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Commentaries.Domain.Models.Comment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AuthorId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Content")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ObjectId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("ObjectTypeId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("PublishedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("StateId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ObjectTypeId");

                    b.HasIndex("StateId");

                    b.HasIndex("ObjectId", "ObjectTypeId");

                    b.ToTable("Comment", "public");
                });

            modelBuilder.Entity("Commentaries.Domain.Models.CommentFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("CommentId")
                        .HasColumnType("uuid");

                    b.Property<byte[]>("Data")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<DateTime>("DeletedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("UploadTimestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("CommentId");

                    b.ToTable("CommentFile", "public");
                });

            modelBuilder.Entity("Commentaries.Domain.Models.CommentState", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.ToTable("CommentState", "public");

                    b.HasData(
                        new
                        {
                            Id = 0,
                            Code = "Draft"
                        },
                        new
                        {
                            Id = 1,
                            Code = "Published"
                        },
                        new
                        {
                            Id = 2,
                            Code = "Deleted"
                        });
                });

            modelBuilder.Entity("Commentaries.Domain.Models.ObjectType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.HasKey("Id");

                    b.HasIndex("FullName")
                        .IsUnique();

                    b.ToTable("ObjectType", "public");
                });

            modelBuilder.Entity("Commentaries.Domain.Models.Comment", b =>
                {
                    b.HasOne("Commentaries.Domain.Models.ObjectType", "ObjectType")
                        .WithMany()
                        .HasForeignKey("ObjectTypeId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Commentaries.Domain.Models.CommentState", "State")
                        .WithMany()
                        .HasForeignKey("StateId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("ObjectType");

                    b.Navigation("State");
                });

            modelBuilder.Entity("Commentaries.Domain.Models.CommentFile", b =>
                {
                    b.HasOne("Commentaries.Domain.Models.Comment", "Comment")
                        .WithMany("Files")
                        .HasForeignKey("CommentId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Comment");
                });

            modelBuilder.Entity("Commentaries.Domain.Models.Comment", b =>
                {
                    b.Navigation("Files");
                });
#pragma warning restore 612, 618
        }
    }
}