using MagicVilla_Web.Models.DTO;

namespace MagicVilla_Web.Services
{
    public interface IVillaService
    {
        Task<T> ObtenerTodos<T>();
        Task<T> Obtener<T>(int Id);
        Task<T> Crear<T>(VillaCreateDTO DTO);
        Task<T> Actualizar<T>(VillaUpdateDTO DTO);
        Task<T> Remover<T>(int Id);

    }
}
