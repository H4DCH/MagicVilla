using MagicVilla_Utilidad;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class VillaService : BaseService, IVillaService
    {
        private readonly IHttpClientFactory httpClient;
        private string _villaUrl;

        public VillaService(IHttpClientFactory httpClient,IConfiguration configuration) : base(httpClient)
        {
            this.httpClient = httpClient;
            _villaUrl = configuration.GetValue<String>("ServiceUrls:API_URL");   
        }
        public Task<T> Actualizar<T>(VillaUpdateDTO dto)
        {
            return SenAsync<T>(new APIRequest()
            {
                APITipo = DS.APITIPO.PUT,
                Datos = dto,
                Url = _villaUrl + "/api/villa/"+dto.Id
            });
        }

        public Task<T> Crear<T>(VillaCreateDTO dto)
        {
            return SenAsync<T>(new APIRequest()
            {
                APITipo = DS.APITIPO.POST,
                Datos= dto,
                Url = _villaUrl+"/api/villa"
            });
        }

        public Task<T> Obtener<T>(int Id)
        {
            return SenAsync<T>(new APIRequest()
            {
                APITipo = DS.APITIPO.GET,
                Url = _villaUrl +"/api/villa/"+Id
            });
        }

        public Task<T> ObtenerTodos<T>()
        {
            return SenAsync<T>(new APIRequest()
            {
                APITipo = DS.APITIPO.GET,
                Url = _villaUrl + "/api/villa/"
            });
        }

        public Task<T> Remover<T>(int Id)
        {
            return SenAsync<T>(new APIRequest()
            {
                APITipo = DS.APITIPO.DELETE,
                Url = _villaUrl + "/api/villa/" + Id
            });
        }
    }
}
