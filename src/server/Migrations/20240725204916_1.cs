using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "metrics_current",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    datetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    latency = table.Column<long>(type: "bigint", nullable: false),
                    value = table.Column<decimal>(type: "numeric", nullable: true),
                    message = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metrics_current", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "stacks",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stacks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "devices",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    host = table.Column<string>(type: "text", nullable: false),
                    Stackid = table.Column<string>(type: "text", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "sensors",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    check_each = table.Column<int>(type: "integer", nullable: false),
                    host = table.Column<string>(type: "text", nullable: true),
                    port = table.Column<int>(type: "integer", nullable: true),
                    timeout = table.Column<int>(type: "integer", nullable: false),
                    username = table.Column<string>(type: "text", nullable: true),
                    password = table.Column<string>(type: "text", nullable: true),
                    SSL_Verification_Check = table.Column<bool>(type: "boolean", nullable: true),
                    Deviceid = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sensors", x => x.id);
                    table.ForeignKey(
                        name: "FK_sensors_devices_Deviceid",
                        column: x => x.Deviceid,
                        principalTable: "devices",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "channels",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    channel_id = table.Column<int>(type: "integer", nullable: false),
                    unit = table.Column<string>(type: "text", nullable: true),
                    success_value = table.Column<decimal>(type: "numeric", nullable: false),
                    success_orientation = table.Column<int>(type: "integer", nullable: false),
                    warning_value = table.Column<decimal>(type: "numeric", nullable: false),
                    warning_orientation = table.Column<int>(type: "integer", nullable: false),
                    danger_value = table.Column<decimal>(type: "numeric", nullable: false),
                    danger_orientation = table.Column<int>(type: "integer", nullable: false),
                    current_metricid = table.Column<string>(type: "text", nullable: true),
                    Sensorid = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_channels", x => x.id);
                    table.ForeignKey(
                        name: "FK_channels_metrics_current_current_metricid",
                        column: x => x.current_metricid,
                        principalTable: "metrics_current",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_channels_sensors_Sensorid",
                        column: x => x.Sensorid,
                        principalTable: "sensors",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "metrics_history",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    datetime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    latency = table.Column<long>(type: "bigint", nullable: false),
                    value = table.Column<decimal>(type: "numeric", nullable: true),
                    Channelid = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_metrics_history", x => x.id);
                    table.ForeignKey(
                        name: "FK_metrics_history_channels_Channelid",
                        column: x => x.Channelid,
                        principalTable: "channels",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_channels_current_metricid",
                table: "channels",
                column: "current_metricid");

            migrationBuilder.CreateIndex(
                name: "IX_channels_Sensorid",
                table: "channels",
                column: "Sensorid");

            migrationBuilder.CreateIndex(
                name: "IX_devices_Stackid",
                table: "devices",
                column: "Stackid");

            migrationBuilder.CreateIndex(
                name: "IX_metrics_history_Channelid",
                table: "metrics_history",
                column: "Channelid");

            migrationBuilder.CreateIndex(
                name: "IX_sensors_Deviceid",
                table: "sensors",
                column: "Deviceid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "metrics_history");

            migrationBuilder.DropTable(
                name: "channels");

            migrationBuilder.DropTable(
                name: "metrics_current");

            migrationBuilder.DropTable(
                name: "sensors");

            migrationBuilder.DropTable(
                name: "devices");

            migrationBuilder.DropTable(
                name: "stacks");
        }
    }
}
