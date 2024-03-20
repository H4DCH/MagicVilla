using MagicVilla_API.Modelos;

namespace MagicVilla_API.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {
        Task<Villa> Actualizar(Villa entidad);
    }
}
