﻿// <auto-generated />
using System;
using MagicVilla_API.Datos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MagicVilla_API.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20240317034544_Ingreso Datos")]
    partial class IngresoDatos
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MagicVilla_API.Modelos.Villa", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Amenidad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Detalle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FechaActualizacion")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImagenUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("MetrosCuadrados")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Ocupantes")
                        .HasColumnType("int");

                    b.Property<double>("Tarifa")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Villas");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Amenidad = "",
                            Detalle = "Detalle de la villa...",
                            FechaActualizacion = new DateTime(2024, 3, 16, 22, 45, 44, 564, DateTimeKind.Local).AddTicks(3866),
                            FechaCreacion = new DateTime(2024, 3, 16, 22, 45, 44, 564, DateTimeKind.Local).AddTicks(3853),
                            ImagenUrl = "",
                            MetrosCuadrados = 80,
                            Nombre = "Villa Real",
                            Ocupantes = 3,
                            Tarifa = 100000.0
                        },
                        new
                        {
                            Id = 2,
                            Amenidad = "",
                            Detalle = "Detalle de la villa...",
                            FechaActualizacion = new DateTime(2024, 3, 16, 22, 45, 44, 564, DateTimeKind.Local).AddTicks(3870),
                            FechaCreacion = new DateTime(2024, 3, 16, 22, 45, 44, 564, DateTimeKind.Local).AddTicks(3870),
                            ImagenUrl = "",
                            MetrosCuadrados = 100,
                            Nombre = "Villa Premium",
                            Ocupantes = 5,
                            Tarifa = 350000.0
                        });
                });
#pragma warning restore 612, 618
        }
    }
}