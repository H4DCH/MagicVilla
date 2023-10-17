using AutoMapper;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.DTO;
using MagicVilla_Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.Hosting;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public VillaController(IVillaService villaService,IMapper mapper)
        {
            _villaService = villaService;   
            _mapper = mapper;   
        }
        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> villaList = new();

            var response = await _villaService.ObtenerTodos<ApiResponse>();
            
              if (response != null && response.IsExitoso) 
            {
                villaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Resultado));
            }
            
            return View(villaList);
        }
        public async Task<IActionResult> CrearVilla() 
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CrearVilla(VillaCreateDTO modelo) 
        {
            if (ModelState.IsValid) 
            {
                var response = await _villaService.Crear<ApiResponse>(modelo);
                if (response != null && response.IsExitoso) 
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            return View(modelo);  
        }
        public async Task<IActionResult> ActualizarVilla(int Id) 
        {
            var response = await _villaService.Obtener<ApiResponse>(Id);

            if(response != null && response.IsExitoso)
            {
                VillaDTO modelo = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Resultado));
                return View(_mapper.Map<VillaUpdateDTO>(modelo));
            }
            return NotFound();
        }
        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> ActualizarVilla(VillaUpdateDTO modelo)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.Actualizar<ApiResponse>(modelo);
                if(response!=null && response.IsExitoso)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            return View(modelo);
        }
        public async Task<IActionResult> EliminarVilla(int Id)
        {
            var response = await _villaService.Obtener<ApiResponse>(Id);

            if (response != null && response.IsExitoso)
            {
                VillaDTO modelo = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Resultado));
                return View(modelo);
            }
            return NotFound();
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarVilla(VillaDTO modelo)
        {
                var response = await _villaService.Remover<ApiResponse>(modelo.Id);

                if (response != null && response.IsExitoso)
                {
                    return RedirectToAction(nameof(IndexVilla));
                }

            return View(modelo);
        }
    }
}
