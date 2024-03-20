using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Repository.IRepository;

namespace MagicVilla_API.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly Context context;

        public VillaRepository(Context context) : base(context)
        {
            this.context = context;
        }
        public async Task<Villa> Actualizar(Villa entidad)
        {
            entidad.FechaActualizacion = DateTime.Now;
            context.Villas.Update(entidad);
            await context.SaveChangesAsync();

            return entidad;
        }
    }
}
