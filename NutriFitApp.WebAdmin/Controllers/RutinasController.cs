using Microsoft.AspNetCore.Mvc;
using NutriFitApp.Shared.DTOs;
using NutriFitApp.WebAdmin.ViewModels;
using NutriFitApp.WebAdmin.Helpers;
using System.Net.Http.Json;

namespace NutriFitApp.WebAdmin.Controllers
{
    public class RutinasController : Controller
    {
        private readonly HttpClient _http;

        public RutinasController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // Mostrar lista de rutinas
        public async Task<IActionResult> Index()
        {
            var rutinas = await _http.GetFromJsonAsync<List<RutinaDTO>>("rutinas");
            return View(rutinas);
        }

        // Mostrar formulario
        [HttpGet]
        public IActionResult Asignar()
        {
            return View(new AsignarRutinaViewModel());
        }

        // Procesar formulario
        [HttpPost]
        public async Task<IActionResult> Asignar(AsignarRutinaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new AsignacionRutinaDTO
            {
                UsuarioId = model.UsuarioId,
                EntrenadorId = model.EntrenadorId,
                Descripcion = model.Descripcion,
                DuracionDias = model.DuracionDias
            };

            var response = await _http.PostAsJsonAsync("rutinas/asignar", dto);

            if (response.IsSuccessStatusCode)
            {
                AlertHelper.Success(TempData, "Rutina asignada correctamente.");
                return RedirectToAction("Index");
            }

            AlertHelper.Error(TempData, "Error al asignar la rutina.");
            return View(model);
        }
    }
}
