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
    [Route("api/numerovilla")]
    [ApiController]
    public class NumeroVillaController: ControllerBase
    {
        private readonly ILogger<NumeroVillaController> logger;
        private readonly INumeroVillaRepository numeroVillaRepos;
        private readonly IVillaRepository villaRepos;
        private readonly IMapper mapper;
        private readonly APIResponse response;

        public NumeroVillaController(ILogger<NumeroVillaController> logger,INumeroVillaRepository numeroVillaRepos, IMapper mapper
            ,IVillaRepository villaRepos)
        {
            this.logger = logger;
            this.numeroVillaRepos = numeroVillaRepos;
            this.villaRepos = villaRepos;
            this.mapper = mapper;
            response = new ();
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetNumeroVillas()
        {
            try
            {
                logger.LogInformation("Obteniendo numeros de  las villas");
                List<NumeroVilla> numeroVillas= await numeroVillaRepos.ObtenerTodos();
                response.Resultado = mapper.Map<List<VillaDTO>>(numeroVillas);
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

        [HttpGet("{Id:int}",Name ="NumeroVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetNumerVillaId(int Id)
        {
            try
            {
                var numeroVilla = await numeroVillaRepos.Obtener(x => x.VillaNo == Id);

                if (Id == 0)
                {
                    logger.LogError($"Error al traer la numero de villa con Id :{Id}");
                    response.statusCode = HttpStatusCode.BadRequest;
                    response.IsExitos = false;
                    return BadRequest(response);
                }
                if (numeroVilla == null)
                {
                    response.statusCode = HttpStatusCode.NotFound;
                    response.IsExitos = false;
                    return NotFound(response);
                }
                response.Resultado = mapper.Map<Villa>(numeroVilla);
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
        public async Task<ActionResult<APIResponse>> CreateNumVilla([FromBody]NumeroVillaCreateDTO createDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await numeroVillaRepos.Obtener(v => v.VillaNo == createDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("NumeroExiste", "El numero ingresado ya existe");
                    return BadRequest(ModelState);
                }
                if (await villaRepos.Obtener(v=>v.Id==createDTO.VillaId)== null)
                {
                    ModelState.AddModelError("IdVilla", "El Id de la villa no existe");
                    return BadRequest(ModelState);
                }

                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
                NumeroVilla modelo = mapper.Map<NumeroVilla>(createDTO);

                modelo.FechaCreacion = DateTime.Now;
                modelo.FechaActualizacion = DateTime.Now;

                await numeroVillaRepos.Crear(modelo);
                response.Resultado = modelo;
                response.statusCode = HttpStatusCode.Created;
                return CreatedAtRoute("NumeroVilla", new { Id = modelo.VillaNo }, response);

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
        public async Task<ActionResult<APIResponse>> DeleteNumeroVilla(int Id)
        {
            try
            {
                if (Id == 0)
                {
                    response.IsExitos = false;
                    response.statusCode = HttpStatusCode.BadRequest;
                    return BadRequest(response);
                }
                var numeroVilla = await numeroVillaRepos.Obtener(v => v.VillaNo == Id);
                if (numeroVilla == null)
                {
                    response.IsExitos = false;
                    response.statusCode = HttpStatusCode.NotFound;
                    return NotFound(response);
                }
                await numeroVillaRepos.Remover(numeroVilla);
                response.statusCode = HttpStatusCode.NoContent; 
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

        [HttpPut("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateNumeroVilla(int Id, [FromBody] NumeroVillaUpdateDTO updateDTO)
        {
            if (updateDTO == null || Id!= updateDTO.VillaNo)
            {
                response.IsExitos = false;
                response.statusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }
            if(await villaRepos.Obtener(v=>v.Id == updateDTO.VillaId) == null)
            {
                ModelState.AddModelError("IdVilla", "El ID de la villa no existe");
                return BadRequest(ModelState);
            }

            NumeroVilla modelo = mapper.Map<NumeroVilla>(updateDTO);

            await numeroVillaRepos.Actualizar(modelo);
            response.statusCode = HttpStatusCode.NoContent;
            return Ok(response);

        }

    }
}
