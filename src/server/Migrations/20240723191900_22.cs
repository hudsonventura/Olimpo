using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class _22 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "danger",
                table: "channels",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "danger_orientation",
                table: "channels",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "success",
                table: "channels",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "success_orientation",
                table: "channels",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "warning",
                table: "channels",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "warning_orientation",
                table: "channels",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "danger",
                table: "channels");

            migrationBuilder.DropColumn(
                name: "danger_orientation",
                table: "channels");

            migrationBuilder.DropColumn(
                name: "success",
                table: "channels");

            migrationBuilder.DropColumn(
                name: "success_orientation",
                table: "channels");

            migrationBuilder.DropColumn(
                name: "warning",
                table: "channels");

            migrationBuilder.DropColumn(
                name: "warning_orientation",
                table: "channels");
        }
    }
}
