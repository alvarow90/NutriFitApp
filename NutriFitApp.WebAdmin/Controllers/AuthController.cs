using Microsoft.AspNetCore.Mvc;
using NutriFitApp.Shared.DTOs;
using System.Net.Http.Json;

namespace NutriFitApp.WebAdmin.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _http;

        public AuthController(IHttpClientFactory factory)
        {
            _http = factory.CreateClient("ApiClient");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginDTO());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var response = await _http.PostAsJsonAsync("auth/login", dto);
            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Credenciales inválidas");
                return View(dto);
            }

            var tokenDto = await response.Content.ReadFromJsonAsync<TokenDTO>();

            // Puedes guardar el token en TempData, Session o Cookie
            TempData["JWT"] = tokenDto?.Token;

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            TempData.Clear();
            return RedirectToAction("Login");
        }
    }
}
