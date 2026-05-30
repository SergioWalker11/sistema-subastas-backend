using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SistemaSubastaBackend.Migrations
{
    /// <inheritdoc />
    public partial class MigracionCompleta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaLimitePago",
                table: "subastas",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GanadorId",
                table: "subastas",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_subastas_GanadorId",
                table: "subastas",
                column: "GanadorId");

            migrationBuilder.AddForeignKey(
                name: "FK_subastas_usuarios_GanadorId",
                table: "subastas",
                column: "GanadorId",
                principalTable: "usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_subastas_usuarios_GanadorId",
                table: "subastas");

            migrationBuilder.DropIndex(
                name: "IX_subastas_GanadorId",
                table: "subastas");

            migrationBuilder.DropColumn(
                name: "FechaLimitePago",
                table: "subastas");

            migrationBuilder.DropColumn(
                name: "GanadorId",
                table: "subastas");
        }
    }
}
