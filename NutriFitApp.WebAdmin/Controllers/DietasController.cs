using Microsoft.AspNetCore.Mvc;
using NutriFitApp.Shared.DTOs;
using NutriFitApp.WebAdmin.ViewModels;
using NutriFitApp.WebAdmin.Helpers;
using System.Net.Http.Json;

namespace NutriFitApp.WebAdmin.Controllers
{
    public class DietasController : Controller
    {
        private readonly HttpClient _http;

        public DietasController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // Mostrar lista de dietas
        public async Task<IActionResult> Index()
        {
            var dietas = await _http.GetFromJsonAsync<List<DietaDTO>>("dietas");
            return View(dietas);
        }

        // Mostrar formulario para asignar una dieta
        [HttpGet]
        public IActionResult Asignar()
        {
            return View(new AsignarDietaViewModel());
        }

        // Procesar el formulario
        [HttpPost]
        public async Task<IActionResult> Asignar(AsignarDietaViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new AsignacionDietaDTO
            {
                UsuarioId = model.UsuarioId,
                NutriologoId = model.NutriologoId,
                Descripcion = model.Descripcion,
                FechaInicio = model.FechaInicio,
                FechaFin = model.FechaFin
            };

            var response = await _http.PostAsJsonAsync("dietas/asignar", dto);

            if (response.IsSuccessStatusCode)
            {
                AlertHelper.Success(TempData, "Dieta asignada correctamente.");
                return RedirectToAction("Index");
            }

            AlertHelper.Error(TempData, "Error al asignar la dieta.");
            return View(model);
        }
    }
}
