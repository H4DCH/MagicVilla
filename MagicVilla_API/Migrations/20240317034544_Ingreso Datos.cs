using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class IngresoDatos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Detalle", "FechaActualizacion", "FechaCreacion", "ImagenUrl", "MetrosCuadrados", "Nombre", "Ocupantes", "Tarifa" },
                values: new object[,]
                {
                    { 1, "", "Detalle de la villa...", new DateTime(2024, 3, 16, 22, 45, 44, 564, DateTimeKind.Local).AddTicks(3866), new DateTime(2024, 3, 16, 22, 45, 44, 564, DateTimeKind.Local).AddTicks(3853), "", 80, "Villa Real", 3, 100000.0 },
                    { 2, "", "Detalle de la villa...", new DateTime(2024, 3, 16, 22, 45, 44, 564, DateTimeKind.Local).AddTicks(3870), new DateTime(2024, 3, 16, 22, 45, 44, 564, DateTimeKind.Local).AddTicks(3870), "", 100, "Villa Premium", 5, 350000.0 }
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
