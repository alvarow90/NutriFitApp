using Microsoft.AspNetCore.Mvc;
using NutriFitApp.Shared.DTOs;
using System.Net.Http.Json;
using NutriFitApp.WebAdmin.Helpers;

namespace NutriFitApp.WebAdmin.Controllers
{
    public class EntrenadoresController : Controller
    {
        private readonly HttpClient _http;

        public EntrenadoresController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // VER
        public async Task<IActionResult> Index()
        {
            var entrenadores = await _http.GetFromJsonAsync<List<UsuarioDTO>>("entrenadores");
            return View(entrenadores);
        }

        // CREAR
        [HttpGet]
        public IActionResult Create()
        {
            return View(new RegisterDTO { Rol = "Entrenador" });
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterDTO dto)
        {
            dto.Rol = "Entrenador";
            var response = await _http.PostAsJsonAsync("auth/register", dto);

            if (response.IsSuccessStatusCode)
            {
                AlertHelper.Success(TempData, "Entrenador creado correctamente.");
                return RedirectToAction("Index");
            }

            AlertHelper.Error(TempData, "Error al crear el entrenador.");
            return View(dto);
        }

        // EDITAR
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var entrenador = await _http.GetFromJsonAsync<UsuarioDTO>($"usuarios/{id}");
            return View(entrenador);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UsuarioDTO model)
        {
            model.Rol = "Entrenador";
            var response = await _http.PutAsJsonAsync($"usuarios/{model.Id}", model);

            if (response.IsSuccessStatusCode)
            {
                AlertHelper.Success(TempData, "Entrenador actualizado correctamente.");
                return RedirectToAction("Index");
            }

            AlertHelper.Error(TempData, "Error al actualizar el entrenador.");
            return View(model);
        }

        // ELIMINAR
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var entrenador = await _http.GetFromJsonAsync<UsuarioDTO>($"usuarios/{id}");
            return View(entrenador);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _http.DeleteAsync($"usuarios/{id}");

            if (response.IsSuccessStatusCode)
            {
                AlertHelper.Success(TempData, "Entrenador eliminado correctamente.");
                return RedirectToAction("Index");
            }

            AlertHelper.Error(TempData, "Error al eliminar el entrenador.");
            var entrenador = await _http.GetFromJsonAsync<UsuarioDTO>($"usuarios/{id}");
            return View(entrenador);
        }
    }
}
