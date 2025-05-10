using Microsoft.AspNetCore.Mvc;
using NutriFitApp.Shared.DTOs;
using System.Net.Http.Json;


namespace NutriFitApp.WebAdmin.Helpers
{
    public static class JwtHelper
    {
        private const string TokenKey = "JWT";

        // Guarda el token en TempData
        public static void GuardarToken(Controller controller, string token)
        {
            controller.TempData[TokenKey] = token;
        }

        // Recupera el token
        public static string? ObtenerToken(Controller controller)
        {
            return controller.TempData[TokenKey] as string;
        }

        // Opcional: pasar el token a las solicitudes
        public static void AgregarToken(HttpClient client, string? token)
        {
            if (!string.IsNullOrWhiteSpace(token))
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }
    }
}
