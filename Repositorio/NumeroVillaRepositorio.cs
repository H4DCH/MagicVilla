using MagicVilla.Datos;
using MagicVilla.Models;
using MagicVilla.Repositorio.IRepositorio;

namespace MagicVilla.Repositorio
{
    public class NumeroVillaRepositorio : Repositorio<NumeroVilla>, INumeroVillaRepositorio
    {
        private readonly Context _Context;
        public NumeroVillaRepositorio(Context context) : base (context)
        {
            _Context = context;
            
        }
        public async Task<NumeroVilla> Actualizar(NumeroVilla entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            _Context.NumeroVillas.Update(entidad);    
            await _Context.SaveChangesAsync();
            return entidad; 
        }
    }
}
