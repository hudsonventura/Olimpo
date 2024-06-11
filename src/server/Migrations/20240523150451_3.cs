using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class _3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_channels_alerts_alertsid",
                table: "channels");

            migrationBuilder.AlterColumn<Guid>(
                name: "alertsid",
                table: "channels",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_channels_alerts_alertsid",
                table: "channels",
                column: "alertsid",
                principalTable: "alerts",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_channels_alerts_alertsid",
                table: "channels");

            migrationBuilder.AlterColumn<Guid>(
                name: "alertsid",
                table: "channels",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_channels_alerts_alertsid",
                table: "channels",
                column: "alertsid",
                principalTable: "alerts",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
