using MagicVilla_Utilidad;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace MagicVilla_Web.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }

        public IHttpClientFactory httpClient { get; set; }
        public BaseService(IHttpClientFactory httpClient)
        {
            this.responseModel = new();
            this.httpClient = httpClient;   

        }
        public async Task<T> SenAsync<T>(APIRequest APIRequest)
        {
            try
            {
                var client = httpClient.CreateClient("MagicAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept","application/json");
                message.RequestUri = new Uri(APIRequest.Url);

                if (APIRequest.Datos!=null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(APIRequest.Datos),
                        Encoding.UTF8,"application/json");
                }
                switch (APIRequest.APITipo)
                {
                    case DS.APITIPO.GET:
                        message.Method = HttpMethod.Get;
                        break;
                    case DS.APITIPO.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case DS.APITIPO.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case DS.APITIPO.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null;
                apiResponse = await client.SendAsync(message);
                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var APIRESPONSE = JsonConvert.DeserializeObject<T>(apiContent);

                return APIRESPONSE;
            }
            catch (Exception ex)
            {
                var dto = new APIResponse
                {
                    ErrorMessages = new List<string> { Convert.ToString(ex.Message) },
                    IsExitos = false
                };
                var res = JsonConvert.SerializeObject(dto);
                var APIresponse = JsonConvert.DeserializeObject<T>(res);

                return APIresponse;
            }
        }
    }
}
