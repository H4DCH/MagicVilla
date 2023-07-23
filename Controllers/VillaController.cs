using MagicVilla.Datos;
using MagicVilla.Models;
using MagicVilla.Models.DTO;
using MagicVilla.VillaStore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Hosting;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MagicVilla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly  Context _context;

        public VillaController(ILogger<VillaController> logger, Context context) 
        {
            _logger = logger;
            _context = context;
        }    
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<VillaDTO>> GetVilla()
        {
            _logger.LogInformation("Obtener todas las villas");
            return Ok(_context.Villas.ToList());

        }

        [HttpGet("Id:int", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(404)]
        public ActionResult<VillaDTO> GetVilla(int Id)
        {
            if (Id == 0)
            {
                _logger.LogError("Error al traer la villa con ID:" +Id);
                return BadRequest();

            }
            //var Villa = VillaStore.VillaStore.Villalista.FirstOrDefault(v => v.Id == Id);
            var Villa = _context.Villas.FirstOrDefault(v => v.Id == Id);
            if (Villa == null)
            {
                return NotFound();
            }
            return Ok(Villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CrearVilla([FromBody] VillaDTO villaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if(_context.Villas.FirstOrDefault(v => v.Nombre.ToLower() == villaDTO.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste","La villa con ese nombre ya existe");
                return BadRequest(ModelState);
            }
            if (villaDTO == null)
            {
                return BadRequest();
            }
            if (villaDTO.Id > 0) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            Villa Modelo = new()
            {
                Nombre = villaDTO.Nombre,
                Detalle = villaDTO.Detalle,
                ImaUrl = villaDTO.ImaUrl,
                Ocupantes = villaDTO.Ocupantes,
                Tarifa = villaDTO.Tarifa,
                MetrosCuadrados = villaDTO.MetrosCuadrados,
                Amenidad = villaDTO.Amenidad,
            };

            _context.Villas.Add(Modelo);
            _context.SaveChanges();
            return CreatedAtRoute("GetVilla", new {Id = villaDTO.Id},villaDTO);
        }
        [HttpDelete("Id:int")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult EliminarVilla(int Id)
        {
            if(Id == 0)
            {   
                return BadRequest();
            }
            var Villa = _context.Villas.FirstOrDefault(v => v.Id == Id);

            if(Villa == null)
            {
                return NotFound();
            }
            _context.Villas.Remove(Villa);
            _context.SaveChanges();
            return NoContent();

        }
        [HttpPut("Id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult EditarVilla(int Id, [FromBody] VillaDTO villaDTO) 
        { 
            if(villaDTO == null || Id != villaDTO.Id)
            {
                return BadRequest();    
            }
            /*var Villa = _context.Villas.FirstOrDefault(v => v.Id == Id);
            Villa.Nombre = villaDTO.Nombre;
            Villa.Ocupantes = villaDTO.Ocupantes;
            Villa.MetrosCuadrados = villaDTO.MetrosCuadrados;*/

            Villa Modelo = new()
            {
                Id = villaDTO.Id,
                Nombre = villaDTO.Nombre,
                Detalle = villaDTO.Detalle,
                MetrosCuadrados = villaDTO.MetrosCuadrados,
                Ocupantes = villaDTO.Ocupantes,
                Tarifa = villaDTO.Tarifa,
                ImaUrl = villaDTO.ImaUrl,
                Amenidad = villaDTO.Amenidad,
            };
            _context.Villas.Update(Modelo);
            _context.SaveChanges();

            return NoContent(); 
        }
        [HttpPatch("Id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ActualizarVilla(int Id, JsonPatchDocument<VillaDTO> patchDto)
        {
            if (patchDto == null || Id == 0)
            {
                return BadRequest();
            }
            var Villa = _context.Villas.AsNoTracking().FirstOrDefault(v => v.Id == Id);

            VillaDTO villaDTO = new()
            {
                Id= Villa.Id,
                Nombre = Villa.Nombre,
                Detalle = Villa.Detalle,
                Tarifa = Villa.Tarifa,
                ImaUrl = Villa.ImaUrl,  
                Ocupantes = Villa.Ocupantes,
                MetrosCuadrados = Villa.MetrosCuadrados,
                Amenidad = Villa.Amenidad,

            };
            if (Villa == null) return BadRequest();

            patchDto.ApplyTo(villaDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Villa Model = new()
            {
                Id = villaDTO.Id,
                Nombre = villaDTO.Nombre,
                Detalle = villaDTO.Detalle,
                Tarifa = villaDTO.Tarifa,
                ImaUrl = villaDTO.ImaUrl,
                Ocupantes = villaDTO.Ocupantes,
                MetrosCuadrados = villaDTO.MetrosCuadrados,
                Amenidad = villaDTO.Amenidad,
            };
            _context.Villas.Update(Model);
            _context.SaveChanges();

            return NoContent();
        }
    }

} 