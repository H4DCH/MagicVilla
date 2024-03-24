using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Repository.IRepository;

namespace MagicVilla_API.Repository
{
    public class NumeroVillaRepository : Repository<NumeroVilla>, INumeroVillaRepository
    {
        private readonly Context context;

        public NumeroVillaRepository(Context context) : base(context)
        {
            this.context = context;
        }
        public async Task<NumeroVilla> Actualizar(NumeroVilla entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            context.NumeroVillas.Update(entidad);
            await context.SaveChangesAsync();

            return entidad;
        }
    }
}
