using MagicVilla.Datos;
using MagicVilla.Models;
using MagicVilla.Repositorio.IRepositorio;

namespace MagicVilla.Repositorio
{
    public class VillaRepositorio : Repositorio<Villa>, IVillaRepositorio
    {
        private readonly Context _Context;
        public VillaRepositorio(Context context) : base (context)
        {
            _Context = context;
            
        }
        public async Task<Villa> Actualizar(Villa entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _Context.Villas.Update(entidad);    
            await _Context.SaveChangesAsync();
            return entidad; 
        }
    }
}
