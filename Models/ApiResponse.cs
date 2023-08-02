using System.Net;

namespace MagicVilla.Models
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsExitoso { get; set; } = true;
        public List<string> Errores { get; set; }
        public Object Resultado { get; set; }   
    }
}
