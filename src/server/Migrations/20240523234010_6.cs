using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class _6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_channels_metrics_metricid",
                table: "channels");

            migrationBuilder.RenameColumn(
                name: "metricid",
                table: "channels",
                newName: "current_metricid");

            migrationBuilder.RenameIndex(
                name: "IX_channels_metricid",
                table: "channels",
                newName: "IX_channels_current_metricid");

            migrationBuilder.AddColumn<Guid>(
                name: "Channelid",
                table: "metrics",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_metrics_Channelid",
                table: "metrics",
                column: "Channelid");

            migrationBuilder.AddForeignKey(
                name: "FK_channels_metrics_current_metricid",
                table: "channels",
                column: "current_metricid",
                principalTable: "metrics",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_metrics_channels_Channelid",
                table: "metrics",
                column: "Channelid",
                principalTable: "channels",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_channels_metrics_current_metricid",
                table: "channels");

            migrationBuilder.DropForeignKey(
                name: "FK_metrics_channels_Channelid",
                table: "metrics");

            migrationBuilder.DropIndex(
                name: "IX_metrics_Channelid",
                table: "metrics");

            migrationBuilder.DropColumn(
                name: "Channelid",
                table: "metrics");

            migrationBuilder.RenameColumn(
                name: "current_metricid",
                table: "channels",
                newName: "metricid");

            migrationBuilder.RenameIndex(
                name: "IX_channels_current_metricid",
                table: "channels",
                newName: "IX_channels_metricid");

            migrationBuilder.AddForeignKey(
                name: "FK_channels_metrics_metricid",
                table: "channels",
                column: "metricid",
                principalTable: "metrics",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
