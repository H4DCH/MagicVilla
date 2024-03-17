using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.DTO;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MagicVilla_API.Controllers
{
    [Route("api/villa")]
    [ApiController]
    public class VillaController: ControllerBase
    {
        private readonly ILogger<VillaController> logger;
        private readonly Context context;

        public VillaController(ILogger<VillaController> logger,Context context)
        {
            this.logger = logger;
            this.context = context;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            logger.LogInformation("Obteniendo todas las villas");
            return Ok(context.Villas.ToList());
        }

        [HttpGet("{Id:int}",Name ="VillaId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> GetVillaId(int Id)
        {
            var villa = context.Villas.FirstOrDefault(x=>x.Id==Id);

            if (Id == 0)
            {
                logger.LogError($"Error al traer la villa con Id :{Id}");
                return BadRequest();
            }
            if(villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> PostVilla([FromBody]VillaDTO villaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(context.Villas.FirstOrDefault(v=> v.Nombre.ToLower() == villaDTO.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "El nombre ingresado ya existe");
                return BadRequest(ModelState);
            }

            if(villaDTO == null)
            {
                return BadRequest();
            }
            if (villaDTO.Id < 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            Villa modelo = new()
            {
                Nombre=villaDTO.Nombre,
                Detalle= villaDTO.Detalle,
                ImagenUrl = villaDTO.ImagenUrl,
                Ocupantes = villaDTO.Ocupantes,
                Tarifa = villaDTO.Tarifa,
                MetrosCuadrados = villaDTO.MetrosCuadrados,
                Amenidad = villaDTO.Amenidad,
                FechaActualizacion=DateTime.Now,
                FechaCreacion = DateTime.Now
            };
            context.Villas.Add(modelo);
            context.SaveChanges();
            return CreatedAtRoute("VillaId",new {Id = villaDTO.Id},villaDTO);
        }

        [HttpDelete("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult DeleteVilla(int Id)
        {
            if(Id == 0)
            {
                return BadRequest();
            }
            var villa = context.Villas.FirstOrDefault(v => v.Id == Id);
            if(villa == null)
            {
                return NotFound();
            }
            context.Villas.Remove(villa);
            context.SaveChanges();
            return NoContent();
        }

        [HttpPut("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult UpdtaeVilla(int Id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO==null || Id!=villaDTO.Id)
            {
                return BadRequest();
            }

            Villa modelo = new()
            {
                Id = villaDTO.Id,
                Nombre = villaDTO.Nombre,
                Ocupantes = villaDTO.Ocupantes,
                Detalle = villaDTO.Detalle,
                ImagenUrl = villaDTO.ImagenUrl,
                Tarifa= villaDTO.Tarifa,
                MetrosCuadrados = villaDTO.MetrosCuadrados,
                Amenidad = villaDTO.Amenidad

            };
            context.Villas.Update(modelo);
            context.SaveChanges();
            return NoContent();

        }

        [HttpPatch("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult PacthVilla(int Id,JsonPatchDocument<VillaDTO> patchDocument)
        {
            if (PacthVilla == null || Id == 0)
            {
                return BadRequest();
            }
            var villa = context.Villas.AsNoTracking().FirstOrDefault(v => v.Id == Id);

            VillaDTO villaDTO = new()
            {
                Id = villa.Id,
                Nombre =villa.Nombre,
                Detalle = villa.Detalle,
                Ocupantes = villa.Ocupantes,
                ImagenUrl=villa.ImagenUrl,
                MetrosCuadrados=villa.MetrosCuadrados,
                Amenidad = villa.Amenidad,
                Tarifa=villa.Tarifa
            };
            if (villa == null)
            {
                return BadRequest();
            }

            patchDocument.ApplyTo(villaDTO, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Villa model = new()
            {
                Id = villaDTO.Id,
                Nombre = villaDTO.Nombre,
                Detalle = villaDTO.Detalle,
                Ocupantes = villaDTO.Ocupantes,
                ImagenUrl = villaDTO.ImagenUrl,
                MetrosCuadrados = villaDTO.MetrosCuadrados,
                Amenidad = villaDTO.Amenidad,
                Tarifa = villaDTO.Tarifa
            };

            context.Update(model);
            context.SaveChanges();
            return NoContent();

        }
    }
}
