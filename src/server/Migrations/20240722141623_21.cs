using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class _21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_channels_alerts_alertsid",
                table: "channels");

            migrationBuilder.DropTable(
                name: "alerts");

            migrationBuilder.DropIndex(
                name: "IX_channels_alertsid",
                table: "channels");

            migrationBuilder.DropColumn(
                name: "alertsid",
                table: "channels");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "alertsid",
                table: "channels",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "alerts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    critical = table.Column<int>(type: "integer", nullable: false),
                    success = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    warning = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_alerts", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_channels_alertsid",
                table: "channels",
                column: "alertsid");

            migrationBuilder.AddForeignKey(
                name: "FK_channels_alerts_alertsid",
                table: "channels",
                column: "alertsid",
                principalTable: "alerts",
                principalColumn: "id");
        }
    }
}
