using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SimpleCMS.Migrations
{
    /// <inheritdoc />
    public partial class Added_AuthorId_To_CmsContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_AppCmsContents_AuthorId",
                table: "AppCmsContents",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCmsContents_AppAuthors_AuthorId",
                table: "AppCmsContents",
                column: "AuthorId",
                principalTable: "AppAuthors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCmsContents_AppAuthors_AuthorId",
                table: "AppCmsContents");

            migrationBuilder.DropIndex(
                name: "IX_AppCmsContents_AuthorId",
                table: "AppCmsContents");
        }
    }
}
