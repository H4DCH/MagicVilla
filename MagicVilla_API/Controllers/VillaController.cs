using MagicVilla_API.Modelos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MagicVilla_API.Controllers
{
    [Route("api/villa")]
    [ApiController]
    public class VillaController: ControllerBase
    {
        [HttpGet]
        public IEnumerable<Villa> GetVillas()
        {
            return new List<Villa>
            {
                new Villa
                {
                    Id=2,
                    Nombre="Prueba1",
                    FechaCreacion = DateTime.Now
                },
                new Villa
                {
                    Id = 3,
                    Nombre="Prueba2",
                    FechaCreacion=DateTime.Now
                }
            };
        }
    }
}
