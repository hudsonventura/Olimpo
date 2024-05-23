using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class _5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "channel",
                table: "sensors");

            migrationBuilder.AddColumn<Guid>(
                name: "Serviceid",
                table: "sensors",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "stacks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stacks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    host = table.Column<string>(type: "text", nullable: false),
                    Stackid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_services", x => x.id);
                    table.ForeignKey(
                        name: "FK_services_stacks_Stackid",
                        column: x => x.Stackid,
                        principalTable: "stacks",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_sensors_Serviceid",
                table: "sensors",
                column: "Serviceid");

            migrationBuilder.CreateIndex(
                name: "IX_services_Stackid",
                table: "services",
                column: "Stackid");

            migrationBuilder.AddForeignKey(
                name: "FK_sensors_services_Serviceid",
                table: "sensors",
                column: "Serviceid",
                principalTable: "services",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sensors_services_Serviceid",
                table: "sensors");

            migrationBuilder.DropTable(
                name: "services");

            migrationBuilder.DropTable(
                name: "stacks");

            migrationBuilder.DropIndex(
                name: "IX_sensors_Serviceid",
                table: "sensors");

            migrationBuilder.DropColumn(
                name: "Serviceid",
                table: "sensors");

            migrationBuilder.AddColumn<int>(
                name: "channel",
                table: "sensors",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
