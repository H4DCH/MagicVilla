using MagicVilla.Models;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.Datos
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) 
        {
            
               
        }    
        public DbSet<Villa> Villas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData
                (
                new Villa
                {
                    Id = 1,
                    Nombre = "Villa Real",
                    Detalle = "Detalles",
                    ImaUrl = "",
                    Ocupantes = 5,
                    MetrosCuadrados = 50,
                    Tarifa = 300,
                    Amenidad = "",
                    FechaCreacion = DateTime.Now,
                    FechaActualizacion = DateTime.Now,

                },
                    new Villa
                    {
                        Id = 2,
                        Nombre = "Villa Premium",
                        Detalle = "Detalles",
                        ImaUrl = "",
                        Ocupantes = 4,
                        MetrosCuadrados = 40,
                        Tarifa = 250,
                        Amenidad = "",
                        FechaCreacion = DateTime.Now,
                        FechaActualizacion = DateTime.Now,

                    }
                );
            ;
        }

    }
}
