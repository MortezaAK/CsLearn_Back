using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBooksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SubTitle",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "CONVERT(nvarchar(max), [Title]) + ' - ' + CONVERT(nvarchar(max), [Description])",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComputedColumnSql: "CONVERT(nvarchar(max), [Title]) + ' - ' + CONVERT(nvarchar(max), [RegDate], 120)");

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ViewCount = table.Column<int>(type: "int", nullable: false),
                    RegDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    posterImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DownloadLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isDelete = table.Column<int>(type: "int", nullable: false),
                    LikeCount = table.Column<int>(type: "int", nullable: false),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: true, computedColumnSql: "CONVERT(nvarchar(max), [Title]) + ' - ' +CONVERT(nvarchar(max), [ShortDescription]) ")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BookKeywords",
                columns: table => new
                {
                    BookId = table.Column<long>(type: "bigint", nullable: false),
                    KeywordId = table.Column<long>(type: "bigint", nullable: false),
                    id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookKeywords", x => new { x.BookId, x.KeywordId });
                    table.ForeignKey(
                        name: "FK_BookKeywords_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BookKeywords_Keywords_KeywordId",
                        column: x => x.KeywordId,
                        principalTable: "Keywords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BooksPermissions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookId = table.Column<long>(type: "bigint", nullable: false),
                    CategoriesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooksPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BooksPermissions_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BooksPermissions_Categories_CategoriesId",
                        column: x => x.CategoriesId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookKeywords_KeywordId",
                table: "BookKeywords",
                column: "KeywordId");

            migrationBuilder.CreateIndex(
                name: "IX_BooksPermissions_BookId",
                table: "BooksPermissions",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BooksPermissions_CategoriesId",
                table: "BooksPermissions",
                column: "CategoriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BookKeywords");

            migrationBuilder.DropTable(
                name: "BooksPermissions");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.AlterColumn<string>(
                name: "SubTitle",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true,
                computedColumnSql: "CONVERT(nvarchar(max), [Title]) + ' - ' + CONVERT(nvarchar(max), [RegDate], 120)",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true,
                oldComputedColumnSql: "CONVERT(nvarchar(max), [Title]) + ' - ' + CONVERT(nvarchar(max), [Description])");
        }
    }
}
