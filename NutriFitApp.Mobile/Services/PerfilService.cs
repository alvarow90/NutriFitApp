// Archivo: Services/PerfilService.cs
// Ubicación: Proyecto NutriFitApp.Mobile
using NutriFitApp.Shared.DTOs; // Para UsuarioPerfilDTO y ActualizarUsuarioPerfilDTO
using System.Net.Http;
using System.Net.Http.Json; // Para GetFromJsonAsync y PutAsJsonAsync
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.Json;     // Para JsonException

namespace NutriFitApp.Mobile.Services
{
    public class PerfilService : IPerfilService
    {
        private readonly HttpClient _httpClient;
        // IMPORTANTE: Asegúrate de que esta URL sea correcta y coincida con la de otros servicios.
        // Ejemplo para emulador Android y API en https://localhost:7149: "https://10.0.2.2:7149"
        private const string ApiBaseUrl = "https://10.0.2.2:7149"; // ¡VERIFICA Y AJUSTA ESTA URL Y PUERTO!

        public PerfilService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            Debug.WriteLine("[PerfilService] Constructor ejecutado. HttpClient inyectado.");
        }

        // Obtiene el perfil del usuario actual desde la API
        public async Task<UsuarioPerfilDTO?> GetMiPerfilAsync()
        {
            string apiUrl = $"{ApiBaseUrl}/api/Usuarios/perfil";
            Debug.WriteLine($"[PerfilService] Intentando obtener perfil desde: {apiUrl}");

            try
            {
                // El AuthTokenHandler (configurado en MauiProgram.cs) debería adjuntar
                // automáticamente el token JWT a la cabecera Authorization de esta solicitud.
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var perfil = await response.Content.ReadFromJsonAsync<UsuarioPerfilDTO>();
                    Debug.WriteLine($"[PerfilService] Perfil obtenido exitosamente para el usuario ID: {perfil?.Id}");
                    return perfil;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[PerfilService] Error obteniendo perfil. Status: {response.StatusCode}, Content: {errorContent}");
                    // Considera devolver null o lanzar una excepción específica que el ViewModel pueda manejar.
                    return null;
                }
            }
            catch (HttpRequestException httpEx)
            {
                Debug.WriteLine($"[PerfilService] HttpRequestException en GetMiPerfilAsync: {httpEx.Message}");
                return null;
            }
            catch (JsonException jsonEx)
            {
                Debug.WriteLine($"[PerfilService] JsonException en GetMiPerfilAsync: {jsonEx.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[PerfilService] Excepción general en GetMiPerfilAsync: {ex.ToString()}");
                return null;
            }
        }

        // Actualiza el perfil del usuario actual en la API
        public async Task<ActualizarPerfilResult> ActualizarMiPerfilAsync(ActualizarUsuarioPerfilDTO perfilDto)
        {
            string apiUrl = $"{ApiBaseUrl}/api/Usuarios/perfil";
            Debug.WriteLine($"[PerfilService] Intentando actualizar perfil en: {apiUrl}");

            try
            {
                // El AuthTokenHandler adjuntará el token JWT automáticamente.
                var response = await _httpClient.PutAsJsonAsync(apiUrl, perfilDto);

                if (response.IsSuccessStatusCode) // HTTP 204 No Content indica éxito
                {
                    Debug.WriteLine("[PerfilService] Perfil actualizado exitosamente.");
                    return new ActualizarPerfilResult { IsSuccess = true };
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[PerfilService] Error actualizando perfil. Status: {response.StatusCode}, Content: {errorContent}");
                    // Intenta deserializar un mensaje de error si la API lo envía en un formato conocido
                    // o simplemente usa el contenido del error.
                    return new ActualizarPerfilResult { IsSuccess = false, ErrorMessage = string.IsNullOrWhiteSpace(errorContent) ? $"Error del servidor: {response.StatusCode}" : errorContent };
                }
            }
            catch (HttpRequestException httpEx)
            {
                Debug.WriteLine($"[PerfilService] HttpRequestException en ActualizarMiPerfilAsync: {httpEx.Message}");
                return new ActualizarPerfilResult { IsSuccess = false, ErrorMessage = "No se pudo conectar al servidor. Verifica tu conexión." };
            }
            catch (JsonException jsonEx)
            {
                Debug.WriteLine($"[PerfilService] JsonException en ActualizarMiPerfilAsync: {jsonEx.Message}");
                return new ActualizarPerfilResult { IsSuccess = false, ErrorMessage = "Error al procesar la solicitud o respuesta del servidor." };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[PerfilService] Excepción general en ActualizarMiPerfilAsync: {ex.ToString()}");
                return new ActualizarPerfilResult { IsSuccess = false, ErrorMessage = "Ocurrió un error inesperado al actualizar el perfil." };
            }
        }
    }
}
