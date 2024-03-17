using MagicVilla_API.Modelos;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla_API.Datos
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options)
        {
            
        }

        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Nombre = "Villa Real",
                    Detalle="Detalle de la villa...",
                    ImagenUrl ="",
                    Ocupantes = 3,
                    MetrosCuadrados = 80,
                    Tarifa = 100000,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now
                },
                new Villa
                {
                    Id = 2,
                    Nombre = "Villa Premium",
                    Detalle = "Detalle de la villa...",
                    ImagenUrl = "",
                    Ocupantes = 5,
                    MetrosCuadrados = 100,
                    Tarifa = 350000,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now

                });
        }
    }
}
