
using MagicVilla_Utilidad;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;

namespace MagicVilla_Web.Services.IServices
{
    public class VillaService : BaseService, IVillaService
    {
        public  IHttpClientFactory _httpClient;
        private string _villaUrl;
        public VillaService(IHttpClientFactory httpClient, IConfiguration configuration) : base(httpClient) 
        {
            _httpClient = httpClient;
            _villaUrl = configuration.GetValue<string>("ServicesUrl:API_URL");

        }
        public Task<T> Actualizar<T>(VillaUpdateDTO DTO)
        {
            return SendAsync<T>(new APIRequest()
            {

                APITipo = DS.APITipo.PUT,
                Datos = DTO,
                Url = _villaUrl + "/api/Villa/"+DTO.Id
            });

        }

        public Task<T> Crear<T>(VillaCreateDTO DTO)
        {
            return SendAsync<T>(new APIRequest() { 
            
                APITipo = DS.APITipo.POST,
                Datos = DTO,
                Url = _villaUrl+"/api/Villa"

            
            });
            
        }

        public Task<T> Obtener<T>(int Id)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _villaUrl + "/api/Villa/" +Id
            });
        }

        public Task<T> ObtenerTodos<T>()
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.GET,
                Url = _villaUrl + "/api/Villa"
            });
        }

        public Task<T> Remover<T>(int Id)
        {
            return SendAsync<T>(new APIRequest()
            {
                APITipo = DS.APITipo.DELETE,
                Url = _villaUrl + "/api/Villa/" + Id
            });

        }
    }
}
