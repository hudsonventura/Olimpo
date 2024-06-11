using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    public partial class _14 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_services_stacks_upid",
                table: "services");

            migrationBuilder.DropForeignKey(
                name: "FK_stacks_stacks_upid",
                table: "stacks");

            migrationBuilder.DropIndex(
                name: "IX_stacks_upid",
                table: "stacks");

            migrationBuilder.DropColumn(
                name: "upid",
                table: "stacks");

            migrationBuilder.DropColumn(
                name: "order",
                table: "services");

            migrationBuilder.RenameColumn(
                name: "upid",
                table: "services",
                newName: "Stackid");

            migrationBuilder.RenameIndex(
                name: "IX_services_upid",
                table: "services",
                newName: "IX_services_Stackid");

            migrationBuilder.AddForeignKey(
                name: "FK_services_stacks_Stackid",
                table: "services",
                column: "Stackid",
                principalTable: "stacks",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_services_stacks_Stackid",
                table: "services");

            migrationBuilder.RenameColumn(
                name: "Stackid",
                table: "services",
                newName: "upid");

            migrationBuilder.RenameIndex(
                name: "IX_services_Stackid",
                table: "services",
                newName: "IX_services_upid");

            migrationBuilder.AddColumn<Guid>(
                name: "upid",
                table: "stacks",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "order",
                table: "services",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_stacks_upid",
                table: "stacks",
                column: "upid");

            migrationBuilder.AddForeignKey(
                name: "FK_services_stacks_upid",
                table: "services",
                column: "upid",
                principalTable: "stacks",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_stacks_stacks_upid",
                table: "stacks",
                column: "upid",
                principalTable: "stacks",
                principalColumn: "id");
        }
    }
}
