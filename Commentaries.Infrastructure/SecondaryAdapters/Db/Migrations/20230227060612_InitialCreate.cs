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
                name: "OutboxMessageState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessageState", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comment",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AuthorId = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StateId = table.Column<int>(type: "integer", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
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
                name: "OutboxMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Payload = table.Column<byte[]>(type: "bytea", nullable: false),
                    PayloadTypeName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PublishAttemptCount = table.Column<int>(type: "integer", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    StateId = table.Column<int>(type: "integer", nullable: false),
                    TargetQueueName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    RoutingKey = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    ParentActivityId = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    DelayUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutboxMessage_OutboxMessageState_StateId",
                        column: x => x.StateId,
                        principalTable: "OutboxMessageState",
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
                    UploadedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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

            migrationBuilder.InsertData(
                table: "OutboxMessageState",
                columns: new[] { "Id", "Code" },
                values: new object[,]
                {
                    { 1, "New" },
                    { 2, "Published" },
                    { 3, "Error" }
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

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_StateId",
                table: "OutboxMessage",
                column: "StateId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentFile",
                schema: "public");

            migrationBuilder.DropTable(
                name: "OutboxMessage");

            migrationBuilder.DropTable(
                name: "Comment",
                schema: "public");

            migrationBuilder.DropTable(
                name: "OutboxMessageState");

            migrationBuilder.DropTable(
                name: "CommentState",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ObjectType",
                schema: "public");
        }
    }
}
