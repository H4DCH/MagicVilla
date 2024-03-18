using AutoMapper;
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
        private readonly IMapper mapper;

        public VillaController(ILogger<VillaController> logger,Context context,IMapper mapper)
        {
            this.logger = logger;
            this.context = context;
            this.mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            logger.LogInformation("Obteniendo todas las villas");
            List<Villa> listaVilla = await context.Villas.ToListAsync();
            return Ok(mapper.Map<List<VillaDTO>>(listaVilla));
        }

        [HttpGet("{Id:int}",Name ="VillaId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVillaId(int Id)
        {
            var villa = await context.Villas.FirstOrDefaultAsync(x=>x.Id==Id);

            if (Id == 0)
            {
                logger.LogError($"Error al traer la villa con Id :{Id}");
                return BadRequest();
            }
            if(villa == null)
            {
                return NotFound();
            }
            
            return Ok(mapper.Map<Villa>(villa));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> PostVilla([FromBody]VillaCreateDTO createDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(await context.Villas.FirstOrDefaultAsync(v=> v.Nombre.ToLower() == createDTO.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "El nombre ingresado ya existe");
                return BadRequest(ModelState);
            }

            if (createDTO == null)
            {
                return BadRequest(createDTO);
            }
            Villa modelo = mapper.Map<Villa>(createDTO);

            await context.Villas.AddAsync(modelo);
            await context.SaveChangesAsync();
            return CreatedAtRoute("VillaId",new {Id = modelo.Id}, modelo);
        }

        [HttpDelete("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteVilla(int Id)
        {
            if(Id == 0)
            {
                return BadRequest();
            }
            var villa = await context.Villas.FirstOrDefaultAsync(v => v.Id == Id);
            if(villa == null)
            {
                return NotFound();
            }
             context.Villas.Remove(villa);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdtaeVilla(int Id, [FromBody] VillaUpdateDTO updateDTO)
        {
            if (updateDTO == null || Id!= updateDTO.Id)
            {
                return BadRequest();
            }

            Villa modelo = mapper.Map<Villa>(updateDTO);
           
            context.Villas.Update(modelo);
           await context.SaveChangesAsync();
            return NoContent();

        }

        [HttpPatch("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> PacthVilla(int Id,JsonPatchDocument<VillaUpdateDTO> patchDocument)
        {
            if (PacthVilla == null || Id == 0)
            {
                return BadRequest();
            }
            var villa = await context.Villas.AsNoTracking().FirstOrDefaultAsync(v => v.Id == Id);

            VillaUpdateDTO villaDTO = mapper.Map<VillaUpdateDTO>(villa);
                
            if (villa == null)
            {
                return BadRequest();
            }

            patchDocument.ApplyTo(villaDTO, ModelState);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Villa model = mapper.Map<Villa>(villaDTO);

            context.Update(model);
            await context.SaveChangesAsync();
            return NoContent(); 

        }
    }
}
