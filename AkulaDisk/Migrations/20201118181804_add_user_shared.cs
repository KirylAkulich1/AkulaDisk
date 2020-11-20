using Microsoft.EntityFrameworkCore.Migrations;

namespace AkulaDisk.Migrations
{
    public partial class add_user_shared : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "SharedFolders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SharedFolders_UserId",
                table: "SharedFolders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SharedFolders_AspNetUsers_UserId",
                table: "SharedFolders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharedFolders_AspNetUsers_UserId",
                table: "SharedFolders");

            migrationBuilder.DropIndex(
                name: "IX_SharedFolders_UserId",
                table: "SharedFolders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SharedFolders");
        }
    }
}
