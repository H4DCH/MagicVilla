using AutoMapper;
using MagicVilla.Datos;
using MagicVilla.Models;
using MagicVilla.Models.DTO;
using MagicVilla.Repositorio.IRepositorio;
using MagicVilla.VillaStore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Hosting;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MagicVilla.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public VillaController(ILogger<VillaController> logger, IVillaRepositorio VillaRepos,IMapper mapper) 
        {
            _logger = logger;
            _villaRepo = VillaRepos;
            _mapper = mapper;
            _response = new();
        }    
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ApiResponse>> GetVilla()
        {
            try
            {
                _logger.LogInformation("Obtener todas las villas");

                IEnumerable<Villa> VillaList = await _villaRepo.ObtenerTodos();

                _response.Resultado = _mapper.Map<IEnumerable<VillaDTO>>(VillaList);
                _response.StatusCode = HttpStatusCode.OK;

                return Ok(_response);

            }
            catch (Exception e) 
            {
                _response.IsExitoso = false;
                _response.Errores = new List<string>() { e.ToString()};
            }
            return _response;
        }

        [HttpGet("Id:int", Name ="GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ApiResponse>> GetVilla(int Id)
        {
            try
            {
                if (Id == 0)
                {
                    _logger.LogError("Error al traer la villa con ID:" + Id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);

                }
                //var Villa = VillaStore.VillaStore.Villalista.FirstOrDefault(v => v.Id == Id);
                var Villa = await _villaRepo.Obtener(v => v.Id == Id);
                if (Villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<VillaDTO>(Villa);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.Errores = new List<string> { ex.ToString() };
            }

            return _response;
        
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse>> CrearVilla([FromBody] VillaCreateDTO createDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (await _villaRepo.Obtener(v => v.Nombre.ToLower() == createDTO.Nombre.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe");
                    return BadRequest(ModelState);
                }
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
                Villa Modelo = _mapper.Map<Villa>(createDTO);
                Modelo.FechaCreacion = DateTime.Now;
                Modelo.FechaActualizacion = DateTime.Now;

                await _villaRepo.Crear(Modelo);
                _response.Resultado = Modelo;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetVilla", new { Id = Modelo.Id },  _response);

            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.Errores = new List<string> { ex.Message };    
                
            }
            return _response;
            
        }
        [HttpDelete("Id:int")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task <IActionResult> EliminarVilla(int Id)
        {
            try
            {
                if (Id == 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var Villa = await _villaRepo.Obtener(v => v.Id == Id);

                if (Villa == null)
                {
                    _response.IsExitoso= false; 
                    _response.StatusCode = HttpStatusCode.NotFound;   
                    return NotFound(_response);
                }
                await _villaRepo.Remover(Villa);

                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);

            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.Errores = new List<string>(){ ex.ToString() };    
                
            }
            return BadRequest(_response);
        }

        [HttpPut("Id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> EditarVilla(int Id, [FromBody] VillaUpdateDTO updateDTO) 
        { 
            if(updateDTO == null || Id != updateDTO.Id)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);    
            }
            
            Villa Modelo = _mapper.Map<Villa>(updateDTO);

            await _villaRepo.Actualizar(Modelo);
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response); 
        }
        [HttpPatch("Id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task <IActionResult> ActualizarVilla(int Id, JsonPatchDocument<VillaUpdateDTO> patchDto)
        {
            if (patchDto == null || Id == 0)
            {
                return BadRequest();
            }
            var Villa =await _villaRepo.Obtener(v => v.Id == Id, tracked : false);

            VillaUpdateDTO villaDTO = _mapper.Map<VillaUpdateDTO>(Villa);
            
            if (Villa == null) return BadRequest();

            patchDto.ApplyTo(villaDTO, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Villa modelo = _mapper.Map<Villa>(villaDTO);    

            await _villaRepo.Actualizar(modelo);
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
    }

} 