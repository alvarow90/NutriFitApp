using Microsoft.AspNetCore.Mvc;
using NutriFitApp.Shared.DTOs;
using System.Net.Http.Json;
using NutriFitApp.WebAdmin.Helpers;

namespace NutriFitApp.WebAdmin.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly HttpClient _http;

        public UsuariosController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        // ✅ GET: /Usuarios
        public async Task<IActionResult> Index()
        {
            var usuarios = await _http.GetFromJsonAsync<List<UsuarioDTO>>("usuarios");
            return View(usuarios);
        }

        // ✅ GET: /Usuarios/Create
        public IActionResult Create()
        {
            return View(new RegisterDTO());
        }

        // ✅ POST: /Usuarios/Create
        [HttpPost]
        public async Task<IActionResult> Create(RegisterDTO dto)
        {
            var response = await _http.PostAsJsonAsync("auth/register", dto);
            if (response.IsSuccessStatusCode)
            {
                AlertHelper.Success(TempData, "Usuario creado correctamente.");
                return RedirectToAction("Index");
            }

            AlertHelper.Error(TempData, "Error al crear el usuario.");
            return View(dto);
        }

        // ✅ GET: /Usuarios/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await _http.GetFromJsonAsync<UsuarioDTO>($"usuarios/{id}");
            return View(usuario);
        }

        // ✅ POST: /Usuarios/Edit
        [HttpPost]
        public async Task<IActionResult> Edit(UsuarioDTO model)
        {
            var response = await _http.PutAsJsonAsync($"usuarios/{model.Id}", model);
            if (response.IsSuccessStatusCode)
            {
                AlertHelper.Success(TempData, "Usuario actualizado correctamente.");
                return RedirectToAction("Index");
            }

            AlertHelper.Error(TempData, "Error al actualizar el usuario.");
            return View(model);
        }

        // ✅ GET: /Usuarios/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await _http.GetFromJsonAsync<UsuarioDTO>($"usuarios/{id}");
            return View(usuario);
        }

        // ✅ POST: /Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var response = await _http.DeleteAsync($"usuarios/{id}");
            if (response.IsSuccessStatusCode)
            {
                AlertHelper.Success(TempData, "Usuario eliminado correctamente.");
                return RedirectToAction("Index");
            }

            AlertHelper.Error(TempData, "Error al eliminar el usuario.");
            var usuario = await _http.GetFromJsonAsync<UsuarioDTO>($"usuarios/{id}");
            return View(usuario);
        }
    }
}
