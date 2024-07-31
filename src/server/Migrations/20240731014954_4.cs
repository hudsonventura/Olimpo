using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SSL_Verification_Check",
                table: "sensors");

            migrationBuilder.AddColumn<string>(
                name: "config",
                table: "sensors",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "config",
                table: "sensors");

            migrationBuilder.AddColumn<bool>(
                name: "SSL_Verification_Check",
                table: "sensors",
                type: "boolean",
                nullable: true);
        }
    }
}
