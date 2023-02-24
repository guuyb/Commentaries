using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Commentaries.Infrastructure.SecondaryAdapters.Db.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "CommentState",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ObjectType",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObjectType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StateId = table.Column<int>(type: "integer", nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Content = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ObjectId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ObjectTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comment_CommentState_StateId",
                        column: x => x.StateId,
                        principalSchema: "public",
                        principalTable: "CommentState",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comment_ObjectType_ObjectTypeId",
                        column: x => x.ObjectTypeId,
                        principalSchema: "public",
                        principalTable: "ObjectType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CommentFile",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Data = table.Column<byte[]>(type: "bytea", nullable: false),
                    CommentId = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
                    UploadTimestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommentFile_Comment_CommentId",
                        column: x => x.CommentId,
                        principalSchema: "public",
                        principalTable: "Comment",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "CommentState",
                columns: new[] { "Id", "Code" },
                values: new object[,]
                {
                    { 0, "Draft" },
                    { 1, "Published" },
                    { 2, "Deleted" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ObjectId_ObjectTypeId",
                schema: "public",
                table: "Comment",
                columns: new[] { "ObjectId", "ObjectTypeId" });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_ObjectTypeId",
                schema: "public",
                table: "Comment",
                column: "ObjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_StateId",
                schema: "public",
                table: "Comment",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentFile_CommentId",
                schema: "public",
                table: "CommentFile",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_ObjectType_FullName",
                schema: "public",
                table: "ObjectType",
                column: "FullName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentFile",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Comment",
                schema: "public");

            migrationBuilder.DropTable(
                name: "CommentState",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ObjectType",
                schema: "public");
        }
    }
}
