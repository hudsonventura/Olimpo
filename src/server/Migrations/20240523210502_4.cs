using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class _4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Sensorid",
                table: "channels",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "unit",
                table: "channels",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "sensors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    channel = table.Column<int>(type: "integer", nullable: false),
                    port = table.Column<int>(type: "integer", nullable: false),
                    timeout = table.Column<int>(type: "integer", nullable: false),
                    username = table.Column<string>(type: "text", nullable: false),
                    password = table.Column<string>(type: "text", nullable: false),
                    check_each = table.Column<int>(type: "integer", nullable: false),
                    metric_unit = table.Column<int>(type: "integer", nullable: false),
                    SSL_Verification_Check = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sensors", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_channels_Sensorid",
                table: "channels",
                column: "Sensorid");

            migrationBuilder.AddForeignKey(
                name: "FK_channels_sensors_Sensorid",
                table: "channels",
                column: "Sensorid",
                principalTable: "sensors",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_channels_sensors_Sensorid",
                table: "channels");

            migrationBuilder.DropTable(
                name: "sensors");

            migrationBuilder.DropIndex(
                name: "IX_channels_Sensorid",
                table: "channels");

            migrationBuilder.DropColumn(
                name: "Sensorid",
                table: "channels");

            migrationBuilder.DropColumn(
                name: "unit",
                table: "channels");
        }
    }
}
