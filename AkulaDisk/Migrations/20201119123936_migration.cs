using Microsoft.EntityFrameworkCore.Migrations;

namespace AkulaDisk.Migrations
{
    public partial class migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FolderId",
                table: "AddRequests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AddRequests",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AddRequests_FolderId",
                table: "AddRequests",
                column: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_AddRequests_SharedFolders_FolderId",
                table: "AddRequests",
                column: "FolderId",
                principalTable: "SharedFolders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddRequests_SharedFolders_FolderId",
                table: "AddRequests");

            migrationBuilder.DropIndex(
                name: "IX_AddRequests_FolderId",
                table: "AddRequests");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "AddRequests");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AddRequests");
        }
    }
}
