using Microsoft.EntityFrameworkCore.Migrations;

namespace AkulaDisk.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharedFolders_Files_FileModelId",
                table: "SharedFolders");

            migrationBuilder.AddForeignKey(
                name: "FK_SharedFolders_Files_FileModelId",
                table: "SharedFolders",
                column: "FileModelId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharedFolders_Files_FileModelId",
                table: "SharedFolders");

            migrationBuilder.AddForeignKey(
                name: "FK_SharedFolders_Files_FileModelId",
                table: "SharedFolders",
                column: "FileModelId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
