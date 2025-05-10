// Archivo: Services/PerfilService.cs
// Ubicación: Proyecto NutriFitApp.Mobile
using NutriFitApp.Shared.DTOs; // Para UsuarioPerfilDTO y ActualizarUsuarioPerfilDTO
using System.Net.Http;
using System.Net.Http.Json; // Para GetFromJsonAsync y PutAsJsonAsync
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.Json;     // Para JsonException
using System;                // Para Exception y ArgumentNullException

namespace NutriFitApp.Mobile.Services
{
    public class PerfilService : IPerfilService
    {
        private readonly HttpClient _httpClient;
        // Usando localhost porque la app MAUI corre directamente en Windows
        // y la API también corre en localhost en el puerto 7149 con HTTPS.
        private const string ApiBaseUrl = "https://localhost:7149"; // ¡Asegúrate de que el puerto 7149 sea el correcto para tu API!

        public PerfilService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            Debug.WriteLine("[PerfilService] Constructor ejecutado. HttpClient inyectado.");
        }

        // Obtiene el perfil del usuario actual desde la API
        public async Task<UsuarioPerfilDTO?> GetMiPerfilAsync()
        {
            // Apuntando al endpoint /api/Usuarios/myprofile que debería estar protegido con [Authorize]
            string apiUrl = $"{ApiBaseUrl}/api/Usuarios/myprofile";
            Debug.WriteLine($"[PerfilService] Intentando obtener perfil desde: {apiUrl}");

            try
            {
                // El AuthTokenHandler (configurado en MauiProgram.cs) debería adjuntar
                // automáticamente el token JWT a la cabecera Authorization de esta solicitud.
                var response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var perfil = await response.Content.ReadFromJsonAsync<UsuarioPerfilDTO>();
                    Debug.WriteLine($"[PerfilService] Perfil obtenido exitosamente para el usuario ID: {perfil?.Id}, Nombre: {perfil?.Nombre}");
                    return perfil;
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[PerfilService] Error obteniendo perfil. Status: {response.StatusCode}, Content: {errorContent}");
                    return null;
                }
            }
            catch (HttpRequestException httpEx)
            {
                Debug.WriteLine($"[PerfilService] HttpRequestException en GetMiPerfilAsync: {httpEx.Message}");
                if (httpEx.InnerException != null)
                {
                    Debug.WriteLine($"[PerfilService] Inner Exception: {httpEx.InnerException.Message}");
                    // Verificar si el error interno sugiere un problema de SSL/TLS
                    if (httpEx.InnerException.Message.Contains("SSL", StringComparison.OrdinalIgnoreCase) ||
                        httpEx.InnerException.Message.Contains("TLS", StringComparison.OrdinalIgnoreCase) ||
                        httpEx.InnerException.Message.Contains("certificate", StringComparison.OrdinalIgnoreCase) ||
                        httpEx.InnerException.Message.Contains("authentication", StringComparison.OrdinalIgnoreCase)) // Añadido para cubrir más casos
                    {
                        Debug.WriteLine("[PerfilService] SOSPECHA DE PROBLEMA DE CERTIFICADO SSL/TLS o AUTENTICACIÓN DE CONEXIÓN. " +
                                        "Asegúrate de que el certificado de desarrollo de localhost sea de confianza " +
                                        "o que la configuración de bypass SSL en MauiProgram.cs (para DEBUG) esté activa y funcionando correctamente.");
                    }
                }
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
            // Apuntando al endpoint /api/Usuarios/myprofile
            string apiUrl = $"{ApiBaseUrl}/api/Usuarios/myprofile";
            Debug.WriteLine($"[PerfilService] Intentando actualizar perfil en: {apiUrl}");

            if (perfilDto == null)
            {
                Debug.WriteLine("[PerfilService] El DTO para actualizar perfil es null.");
                return new ActualizarPerfilResult { IsSuccess = false, ErrorMessage = "Los datos del perfil para actualizar son inválidos." };
            }

            try
            {
                // El AuthTokenHandler adjuntará el token JWT automáticamente.
                var response = await _httpClient.PutAsJsonAsync(apiUrl, perfilDto);

                if (response.IsSuccessStatusCode) // HTTP 204 No Content indica éxito para PUT si no devuelve contenido
                {
                    Debug.WriteLine("[PerfilService] Perfil actualizado exitosamente.");
                    return new ActualizarPerfilResult { IsSuccess = true };
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[PerfilService] Error actualizando perfil. Status: {response.StatusCode}, Content: {errorContent}");
                    return new ActualizarPerfilResult { IsSuccess = false, ErrorMessage = string.IsNullOrWhiteSpace(errorContent) ? $"Error del servidor: {response.StatusCode}" : errorContent };
                }
            }
            catch (HttpRequestException httpEx)
            {
                Debug.WriteLine($"[PerfilService] HttpRequestException en ActualizarMiPerfilAsync: {httpEx.Message}");
                if (httpEx.InnerException != null)
                {
                    Debug.WriteLine($"[PerfilService] Inner Exception: {httpEx.InnerException.Message}");
                    if (httpEx.InnerException.Message.Contains("SSL", StringComparison.OrdinalIgnoreCase) ||
                        httpEx.InnerException.Message.Contains("TLS", StringComparison.OrdinalIgnoreCase) ||
                        httpEx.InnerException.Message.Contains("certificate", StringComparison.OrdinalIgnoreCase) ||
                        httpEx.InnerException.Message.Contains("authentication", StringComparison.OrdinalIgnoreCase))
                    {
                        Debug.WriteLine("[PerfilService] SOSPECHA DE PROBLEMA DE CERTIFICADO SSL/TLS o AUTENTICACIÓN DE CONEXIÓN. " +
                                        "Asegúrate de que el certificado de desarrollo de localhost sea de confianza " +
                                        "o que la configuración de bypass SSL en MauiProgram.cs (para DEBUG) esté activa y funcionando correctamente.");
                    }
                }
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
