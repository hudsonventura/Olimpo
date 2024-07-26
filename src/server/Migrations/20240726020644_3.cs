using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "danger_orientation",
                table: "channels");

            migrationBuilder.DropColumn(
                name: "danger_value",
                table: "channels");

            migrationBuilder.DropColumn(
                name: "success_orientation",
                table: "channels");

            migrationBuilder.DropColumn(
                name: "success_value",
                table: "channels");

            migrationBuilder.DropColumn(
                name: "warning_orientation",
                table: "channels");

            migrationBuilder.DropColumn(
                name: "warning_value",
                table: "channels");

            migrationBuilder.AddColumn<decimal>(
                name: "lower_error",
                table: "channels",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "lower_warning",
                table: "channels",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "upper_error",
                table: "channels",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "upper_warning",
                table: "channels",
                type: "numeric",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lower_error",
                table: "channels");

            migrationBuilder.DropColumn(
                name: "lower_warning",
                table: "channels");

            migrationBuilder.DropColumn(
                name: "upper_error",
                table: "channels");

            migrationBuilder.DropColumn(
                name: "upper_warning",
                table: "channels");

            migrationBuilder.AddColumn<int>(
                name: "danger_orientation",
                table: "channels",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "danger_value",
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
                name: "success_value",
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

            migrationBuilder.AddColumn<decimal>(
                name: "warning_value",
                table: "channels",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
