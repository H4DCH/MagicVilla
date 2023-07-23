using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla.Migrations
{
    /// <inheritdoc />
    public partial class AgregadoDeDatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImaUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalles", new DateTime(2023, 7, 22, 20, 16, 53, 765, DateTimeKind.Local).AddTicks(4241), new DateTime(2023, 7, 22, 20, 16, 53, 765, DateTimeKind.Local).AddTicks(4229), "", 50, "Villa Real", 5, 300.0 },
                    { 2, "", "Detalles", new DateTime(2023, 7, 22, 20, 16, 53, 765, DateTimeKind.Local).AddTicks(4245), new DateTime(2023, 7, 22, 20, 16, 53, 765, DateTimeKind.Local).AddTicks(4244), "", 40, "Villa Premium", 4, 250.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
