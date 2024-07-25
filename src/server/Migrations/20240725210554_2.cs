using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_channels_metrics_current_current_metricid",
                table: "channels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_metrics_current",
                table: "metrics_current");

            migrationBuilder.RenameTable(
                name: "metrics_current",
                newName: "metrics");

            migrationBuilder.AddPrimaryKey(
                name: "PK_metrics",
                table: "metrics",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_channels_metrics_current_metricid",
                table: "channels",
                column: "current_metricid",
                principalTable: "metrics",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_channels_metrics_current_metricid",
                table: "channels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_metrics",
                table: "metrics");

            migrationBuilder.RenameTable(
                name: "metrics",
                newName: "metrics_current");

            migrationBuilder.AddPrimaryKey(
                name: "PK_metrics_current",
                table: "metrics_current",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_channels_metrics_current_current_metricid",
                table: "channels",
                column: "current_metricid",
                principalTable: "metrics_current",
                principalColumn: "id");
        }
    }
}
