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
    public class NumeroVillalaController : ControllerBase
    {
        private readonly ILogger<NumeroVillalaController> _logger;
        private readonly IVillaRepositorio _villaRepo;
        private readonly INumeroVillaRepositorio _numRepo;
        private readonly IMapper _mapper;
        protected ApiResponse _response;

        public NumeroVillalaController(ILogger<NumeroVillalaController> logger, IVillaRepositorio VillaRepos, INumeroVillaRepositorio NumVilla, IMapper mapper) 
        {
            _logger = logger;
            _villaRepo = VillaRepos;
            _numRepo = NumVilla;
            _mapper = mapper;
            _response = new();
        }    
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ApiResponse>> GetNumeroVilla()
        {
            try
            {
                _logger.LogInformation("Obtener Numero villas");

                IEnumerable<NumeroVilla> NumeroVillaList = await _numRepo.ObtenerTodos();

                _response.Resultado = _mapper.Map<IEnumerable<NumeroVillaDTO>>(NumeroVillaList);
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

        [HttpGet("Id:int", Name ="GetNumeroVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ApiResponse>> GetNumeroVilla(int Id)
        {
            try
            {
                if (Id == 0)
                {
                    _logger.LogError("Error al traer Numero de la villa con ID:" + Id);
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.IsExitoso = false;
                    return BadRequest(_response);

                }
                //var Villa = VillaStore.VillaStore.Villalista.FirstOrDefault(v => v.Id == Id);
                var NumeroVilla = await _numRepo.Obtener(v => v.VillaNo == Id);
                if (NumeroVilla == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.IsExitoso = false;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<NumeroVillaDTO>(NumeroVilla);
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
        public async Task<ActionResult<ApiResponse>> CrearNumeroVilla([FromBody] NumeroVillaDTO createDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                if (await _numRepo.Obtener(v => v.VillaNo == createDTO.VillaNo) != null)
                {
                    ModelState.AddModelError("NumeroExiste", "El numero de villa ya existe");
                    return BadRequest(ModelState);
                }
                if (await _villaRepo.Obtener(v =>v.Id==createDTO.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForanea", "El Id de la villa no existe");
                    return BadRequest(ModelState);

                }
                if (createDTO == null)
                {
                    return BadRequest(createDTO);
                }
                NumeroVilla Modelo = _mapper.Map<NumeroVilla>(createDTO);
                Modelo.FechaCreacion = DateTime.Now;
                Modelo.FechaActualizacion = DateTime.Now;

                await _numRepo.Crear(Modelo);
                _response.Resultado = Modelo;
                _response.StatusCode = HttpStatusCode.Created;
                return CreatedAtRoute("GetNumeroVilla", new { Id = Modelo.VillaNo},  _response);

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
        public async Task <IActionResult> EliminarNumeroVilla(int Id)
        {
            try
            {
                if (Id == 0)
                {
                    _response.IsExitoso = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var NumeroVilla = await _numRepo.Obtener(v => v.VillaNo == Id);

                if (NumeroVilla == null)
                {
                    _response.IsExitoso= false; 
                    _response.StatusCode = HttpStatusCode.NotFound;   
                    return NotFound(_response);
                }
                await _numRepo.Remover(NumeroVilla);

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
        public async Task<IActionResult> EditarNumeroVilla(int Id, [FromBody] NumeroVillaUpdateDTO updateDTO) 
        { 
            if(updateDTO == null || Id != updateDTO.VillaNo)
            {
                _response.IsExitoso = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);    
            }
            if (await _villaRepo.Obtener(V => V.Id == updateDTO.VillaId) == null)
            {
                ModelState.AddModelError("ClaveForanea", "Id de la villa no existe");
                return BadRequest(ModelState);
            }
            
            NumeroVilla Modelo = _mapper.Map<NumeroVilla>(updateDTO);

            await _numRepo.Actualizar(Modelo);
            _response.StatusCode = HttpStatusCode.NoContent;
            return Ok(_response); 
        }
        
    }

} 