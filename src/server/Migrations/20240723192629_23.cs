using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class _23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "warning",
                table: "channels",
                newName: "warning_value");

            migrationBuilder.RenameColumn(
                name: "success",
                table: "channels",
                newName: "success_value");

            migrationBuilder.RenameColumn(
                name: "danger",
                table: "channels",
                newName: "danger_value");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "warning_value",
                table: "channels",
                newName: "warning");

            migrationBuilder.RenameColumn(
                name: "success_value",
                table: "channels",
                newName: "success");

            migrationBuilder.RenameColumn(
                name: "danger_value",
                table: "channels",
                newName: "danger");
        }
    }
}
