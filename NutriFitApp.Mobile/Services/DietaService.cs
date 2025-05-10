// Archivo: Services/DietaService.cs
// Ubicación: Proyecto NutriFitApp.Mobile
using NutriFitApp.Shared.DTOs; // Para DietaDTO
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json; // Para ReadFromJsonAsync
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.Json;     // Para JsonException

namespace NutriFitApp.Mobile.Services
{
    public class DietaService : IDietaService
    {
        private readonly HttpClient _httpClient;
        // IMPORTANTE: Asegúrate de que esta URL sea correcta y coincida con la de AuthService.
        // Ejemplo para emulador Android y API en https://localhost:7149: "https://10.0.2.2:7149"
        private const string ApiBaseUrl = "https://localhost:7149"; // ¡VERIFICA Y AJUSTA ESTA URL Y PUERTO!

        public DietaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            Debug.WriteLine("[DietaService] Constructor ejecutado. HttpClient inyectado.");
        }

        // Implementación del método actualizado para obtener "mis dietas".
        public async Task<List<DietaDTO>> GetMisDietasAsync()
        {
            // Endpoint específico en tu API para obtener las dietas del usuario autenticado.
            string apiUrl = $"{ApiBaseUrl}/api/Dietas/misdietas";
            Debug.WriteLine($"[DietaService] Intentando obtener 'mis dietas' desde: {apiUrl}");

            try
            {
                // El AuthTokenHandler (configurado en MauiProgram.cs) debería adjuntar
                // automáticamente el token JWT a la cabecera Authorization de esta solicitud.
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode) // Si la respuesta es exitosa (ej. HTTP 200 OK)
                {
                    // Leer el contenido de la respuesta y deserializarlo a una lista de DietaDTO.
                    var dietas = await response.Content.ReadFromJsonAsync<List<DietaDTO>>();
                    Debug.WriteLine($"[DietaService] 'Mis dietas' obtenidas exitosamente: {(dietas?.Count ?? 0)} encontradas.");
                    return dietas ?? new List<DietaDTO>(); // Devolver la lista de dietas o una lista vacía si es null.
                }
                else // Si la API devuelve un error
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[DietaService] Error obteniendo 'mis dietas'. Status: {response.StatusCode}, Content: {errorContent}");
                    // Devolver lista vacía en caso de error para que el ViewModel pueda manejarlo.
                    return new List<DietaDTO>();
                }
            }
            catch (HttpRequestException httpEx) // Capturar errores de red.
            {
                Debug.WriteLine($"[DietaService] HttpRequestException en GetMisDietasAsync: {httpEx.Message}");
                return new List<DietaDTO>();
            }
            catch (JsonException jsonEx) // Capturar errores al deserializar el JSON.
            {
                Debug.WriteLine($"[DietaService] JsonException en GetMisDietasAsync: {jsonEx.Message}");
                return new List<DietaDTO>();
            }
            catch (Exception ex) // Capturar cualquier otra excepción inesperada.
            {
                Debug.WriteLine($"[DietaService] Excepción general en GetMisDietasAsync: {ex.ToString()}");
                return new List<DietaDTO>();
            }
        }
    }
}
