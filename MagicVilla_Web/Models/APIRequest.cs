using static MagicVilla_Utilidad.DS;

namespace MagicVilla_Web.Models
{
    public class APIRequest
    {
        public APITIPO APITipo { get; set; } = APITIPO.GET;
        public string Url { get; set; }
        public Object Datos { get; set; }
    }
}
