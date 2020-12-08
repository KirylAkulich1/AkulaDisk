using Microsoft.EntityFrameworkCore.Migrations;

namespace AkulaDisk.Migrations
{
    public partial class is_shared_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isShared",
                table: "Files",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isShared",
                table: "Files");
        }
    }
}
