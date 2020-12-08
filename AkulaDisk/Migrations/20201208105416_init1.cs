using Microsoft.EntityFrameworkCore.Migrations;

namespace AkulaDisk.Migrations
{
    public partial class init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddRequests_SharedFolders_FolderId",
                table: "AddRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_AddRequests_SharedFolders_FolderId",
                table: "AddRequests",
                column: "FolderId",
                principalTable: "SharedFolders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AddRequests_SharedFolders_FolderId",
                table: "AddRequests");

            migrationBuilder.AddForeignKey(
                name: "FK_AddRequests_SharedFolders_FolderId",
                table: "AddRequests",
                column: "FolderId",
                principalTable: "SharedFolders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
