using MagicVilla_Web.Models.DTO;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaService
    {
        Task<T> ObtenerTodos<T>();
        Task<T> Obtener<T>(int Id);
        Task<T> Crear<T>(VillaCreateDTO dto);
        Task<T> Actualizar<T>(VillaUpdateDTO dto);
        Task<T> Remover<T>(int Id);
    }
}
