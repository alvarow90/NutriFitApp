// Archivo: Services/RutinaService.cs
// Ubicación: Proyecto NutriFitApp.Mobile
using NutriFitApp.Shared.DTOs; // Para RutinaDTO
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json; // Para ReadFromJsonAsync
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.Json;     // Para JsonException

namespace NutriFitApp.Mobile.Services
{
    public class RutinaService : IRutinaService
    {
        private readonly HttpClient _httpClient;
        // IMPORTANTE: Asegúrate de que esta URL sea correcta y coincida con la de AuthService y DietaService.
        // Ejemplo para emulador Android y API en https://localhost:7149: "https://10.0.2.2:7149"
        private const string ApiBaseUrl = "https://localhost:7149"; // ¡VERIFICA Y AJUSTA ESTA URL Y PUERTO!

        public RutinaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            Debug.WriteLine("[RutinaService] Constructor ejecutado. HttpClient inyectado.");
        }

        public async Task<List<RutinaDTO>> GetMisRutinasAsync()
        {
            // Endpoint específico en tu API para obtener las rutinas del usuario autenticado.
            // DEBERÁS CREAR ESTE ENDPOINT EN TU RutinasController SI NO EXISTE.
            string apiUrl = $"{ApiBaseUrl}/api/Rutinas/misrutinas";
            Debug.WriteLine($"[RutinaService] Intentando obtener 'mis rutinas' desde: {apiUrl}");

            try
            {
                // El AuthTokenHandler (configurado en MauiProgram.cs) debería adjuntar
                // automáticamente el token JWT a la cabecera Authorization de esta solicitud.
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode) // Si la respuesta es exitosa (ej. HTTP 200 OK)
                {
                    // Leer el contenido de la respuesta y deserializarlo a una lista de RutinaDTO.
                    var rutinas = await response.Content.ReadFromJsonAsync<List<RutinaDTO>>();
                    Debug.WriteLine($"[RutinaService] 'Mis rutinas' obtenidas exitosamente: {(rutinas?.Count ?? 0)} encontradas.");
                    return rutinas ?? new List<RutinaDTO>(); // Devolver la lista o una lista vacía si es null.
                }
                else // Si la API devuelve un error
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[RutinaService] Error obteniendo 'mis rutinas'. Status: {response.StatusCode}, Content: {errorContent}");
                    // Devolver lista vacía en caso de error para que el ViewModel pueda manejarlo.
                    return new List<RutinaDTO>();
                }
            }
            catch (HttpRequestException httpEx) // Capturar errores de red.
            {
                Debug.WriteLine($"[RutinaService] HttpRequestException en GetMisRutinasAsync: {httpEx.Message}");
                return new List<RutinaDTO>();
            }
            catch (JsonException jsonEx) // Capturar errores al deserializar el JSON.
            {
                Debug.WriteLine($"[RutinaService] JsonException en GetMisRutinasAsync: {jsonEx.Message}");
                return new List<RutinaDTO>();
            }
            catch (Exception ex) // Capturar cualquier otra excepción inesperada.
            {
                Debug.WriteLine($"[RutinaService] Excepción general en GetMisRutinasAsync: {ex.ToString()}");
                return new List<RutinaDTO>();
            }
        }
    }
}
