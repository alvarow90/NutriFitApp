using Microsoft.AspNetCore.Mvc;
using NutriFitApp.Shared.DTOs;
using System.Net.Http.Json;
using NutriFitApp.WebAdmin.Helpers;

namespace NutriFitApp.WebAdmin.Controllers
{
    public class NutriologosController : Controller
    {
        private readonly HttpClient _http;

        public NutriologosController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // VER
        public async Task<IActionResult> Index()
        {
            var nutriologos = await _http.GetFromJsonAsync<List<UsuarioDTO>>("nutriologos");
            return View(nutriologos);
        }

        // CREAR
        [HttpGet]
        public IActionResult Create()
        {
            return View(new RegisterDTO { Rol = "Nutriologo" });
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterDTO dto)
        {
            dto.Rol = "Nutriologo";
            var response = await _http.PostAsJsonAsync("auth/register", dto);

            if (response.IsSuccessStatusCode)
            {
                AlertHelper.Success(TempData, "Nutriólogo creado correctamente.");
                return RedirectToAction("Index");
            }

            AlertHelper.Error(TempData, "Error al crear el nutriólogo.");
            return View(dto);
        }

        // EDITAR
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var nutriologo = await _http.GetFromJsonAsync<UsuarioDTO>($"usuarios/{id}");
            return View(nutriologo);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UsuarioDTO model)
        {
            model.Rol = "Nutriologo";
            var response = await _http.PutAsJsonAsync($"usuarios/{model.Id}", model);

            if (response.IsSuccessStatusCode)
            {
                AlertHelper.Success(TempData, "Nutriólogo actualizado correctamente.");
                return RedirectToAction("Index");
            }

            AlertHelper.Error(TempData, "Error al actualizar el nutriólogo.");
            return View(model);
        }

        // ELIMINAR
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var nutriologo = await _http.GetFromJsonAsync<UsuarioDTO>($"usuarios/{id}");
            return View(nutriologo);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _http.DeleteAsync($"usuarios/{id}");

            if (response.IsSuccessStatusCode)
            {
                AlertHelper.Success(TempData, "Nutriólogo eliminado correctamente.");
                return RedirectToAction("Index");
            }

            AlertHelper.Error(TempData, "Error al eliminar el nutriólogo.");
            var nutriologo = await _http.GetFromJsonAsync<UsuarioDTO>($"usuarios/{id}");
            return View(nutriologo);
        }
    }
}
