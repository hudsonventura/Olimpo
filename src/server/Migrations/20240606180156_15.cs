using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class _15 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sensors_services_Serviceid",
                table: "sensors");

            migrationBuilder.DropTable(
                name: "services");

            migrationBuilder.RenameColumn(
                name: "Serviceid",
                table: "sensors",
                newName: "Deviceid");

            migrationBuilder.RenameIndex(
                name: "IX_sensors_Serviceid",
                table: "sensors",
                newName: "IX_sensors_Deviceid");

            migrationBuilder.CreateTable(
                name: "devices",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    host = table.Column<string>(type: "text", nullable: false),
                    Stackid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_devices", x => x.id);
                    table.ForeignKey(
                        name: "FK_devices_stacks_Stackid",
                        column: x => x.Stackid,
                        principalTable: "stacks",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_devices_Stackid",
                table: "devices",
                column: "Stackid");

            migrationBuilder.AddForeignKey(
                name: "FK_sensors_devices_Deviceid",
                table: "sensors",
                column: "Deviceid",
                principalTable: "devices",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_sensors_devices_Deviceid",
                table: "sensors");

            migrationBuilder.DropTable(
                name: "devices");

            migrationBuilder.RenameColumn(
                name: "Deviceid",
                table: "sensors",
                newName: "Serviceid");

            migrationBuilder.RenameIndex(
                name: "IX_sensors_Deviceid",
                table: "sensors",
                newName: "IX_sensors_Serviceid");

            migrationBuilder.CreateTable(
                name: "services",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    Stackid = table.Column<Guid>(type: "uuid", nullable: true),
                    host = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false)
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
    }
}
