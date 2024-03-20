using AutoMapper;
using MagicVilla_API.Datos;
using MagicVilla_API.Modelos;
using MagicVilla_API.Modelos.DTO;
using MagicVilla_API.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;

namespace MagicVilla_API.Controllers
{
    [Route("api/villa")]
    [ApiController]
    public class VillaController: ControllerBase
    {
        private readonly ILogger<VillaController> logger;
        private readonly IVillaRepository villaRepos;
        private readonly IMapper mapper;
        private readonly APIResponse response;

        public VillaController(ILogger<VillaController> logger,IVillaRepository villaRepos,IMapper mapper)
        {
            this.logger = logger;
            this.villaRepos = villaRepos;
            this.mapper = mapper;
            response = new ();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                logger.LogInformation("Obteniendo todas las villas");
                List<Villa> listaVilla = await villaRepos.ObtenerTodos();
                response.Resultado = mapper.Map<List<VillaDTO>>(listaVilla);
                response.statusCode = HttpStatusCode.OK;

                return Ok(response);

            }
            catch (Exception ex)
            {

                response.IsExitos = false;
                response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }
            return response;
            
        }

        [HttpGet("{Id:int}",Name ="VillaId")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaId(int Id)
        {
            try
            {
                var villa = await villaRepos.Obtener(x => x.Id == Id);

                if (Id == 0)
                {
                    logger.LogError($"Error al traer la villa con Id :{Id}");
                    response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                if (villa == null)
                {
                    response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(response);
                }
                response.Resultado = mapper.Map<Villa>(villa);
                response.statusCode = HttpStatusCode.OK;
                return Ok(response);

            }
            catch (Exception ex)
            {
                response.IsExitos = false;
                response.ErrorMessages = new List<string>()
                {
                    ex.ToString()
                };
            }
            return response;
            
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> PostVilla([FromBody]VillaCreateDTO createDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await villaRepos.Obtener(v => v.Nombre.ToLower() == createDTO.Nombre.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreExiste", "El nombre ingresado ya existe");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
                Villa modelo = mapper.Map<Villa>(createDTO);

                await villaRepos.Crear(modelo);
                response.Resultado = modelo;
                response.statusCode = HttpStatusCode.Created;
                return CreatedAtRoute("VillaId", new { Id = modelo.Id }, response);

            }
            catch (Exception ex)
            {
                response.IsExitos = false;
                response.ErrorMessages = new List<string>() {ex.ToString() };
            }
            return response;
            
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
            var villa = await villaRepos.Obtener(v => v.Id == Id);
            if(villa == null)
            {
                return NotFound();
            }
            await villaRepos.Remover(villa);
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

            await villaRepos.Actualizar(modelo);
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
            var villa = await villaRepos.Obtener(v => v.Id == Id,tracked:false);

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

            await villaRepos.Actualizar(model);
            return NoContent(); 

        }
    }
}
