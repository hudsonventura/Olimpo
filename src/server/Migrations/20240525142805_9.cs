using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class _9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "metric_unit",
                table: "sensors");

            migrationBuilder.DropColumn(
                name: "unit",
                table: "metrics");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "metric_unit",
                table: "sensors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "metrics",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
