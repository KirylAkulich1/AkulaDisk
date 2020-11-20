using Microsoft.EntityFrameworkCore.Migrations;

namespace AkulaDisk.Migrations
{
    public partial class fix_shared : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharedFolders_Files_FileId",
                table: "SharedFolders");

            migrationBuilder.DropIndex(
                name: "IX_SharedFolders_FileId",
                table: "SharedFolders");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "SharedFolders");

            migrationBuilder.AlterColumn<string>(
                name: "FileModelId",
                table: "SharedFolders",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "SharedFolders",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_SharedFolders_FileModelId",
                table: "SharedFolders",
                column: "FileModelId",
                unique: true,
                filter: "[FileModelId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_SharedFolders_Files_FileModelId",
                table: "SharedFolders",
                column: "FileModelId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SharedFolders_Files_FileModelId",
                table: "SharedFolders");

            migrationBuilder.DropIndex(
                name: "IX_SharedFolders_FileModelId",
                table: "SharedFolders");

            migrationBuilder.AlterColumn<int>(
                name: "FileModelId",
                table: "SharedFolders",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "SharedFolders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldDefaultValueSql: "NEWID()");

            migrationBuilder.AddColumn<string>(
                name: "FileId",
                table: "SharedFolders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SharedFolders_FileId",
                table: "SharedFolders",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_SharedFolders_Files_FileId",
                table: "SharedFolders",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
