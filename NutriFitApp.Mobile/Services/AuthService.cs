// Archivo: Services/AuthService.cs
// Ubicación: Proyecto NutriFitApp.Mobile
using NutriFitApp.Shared.DTOs; // Para LoginDTO y TokenDTO
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using System.Diagnostics;

namespace NutriFitApp.Mobile.Services
{
    public class AuthService : IAuthService // Asegúrate que IAuthService también esté bien definido
    {
        // _httpClient es no anulable y se inicializa en el constructor.
        private readonly HttpClient _httpClient;

        // IMPORTANTE: Configura esta URL para que apunte a tu API local.
        private const string ApiBaseUrl = "https://localhost:7149"; // ¡VERIFICA Y AJUSTA ESTA URL Y PUERTO!
        private const string AuthTokenKey = "AuthToken_NutriFitApp_v1.2";

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient)); // Asegurar que httpClient no sea null
            Debug.WriteLine("[AuthService] Constructor ejecutado. HttpClient inyectado.");
        }

        public async Task<MobileLoginResult> LoginAsync(LoginDTO loginRequest)
        {
            // Esta implementación siempre devuelve una instancia de MobileLoginResult, nunca null.
            if (loginRequest == null)
            {
                Debug.WriteLine("[AuthService] LoginRequest es null.");
                return new MobileLoginResult { IsSuccess = false, ErrorMessage = "La solicitud de login es inválida." };
            }

            try
            {
                string apiUrl = $"{ApiBaseUrl}/api/auth/login";
                Debug.WriteLine($"[AuthService] Intentando login en: {apiUrl} con Email: {loginRequest.Email}");

                HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    TokenDTO? tokenDto = await response.Content.ReadFromJsonAsync<TokenDTO>();

                    if (tokenDto != null && !string.IsNullOrEmpty(tokenDto.Token))
                    {
                        await SecureStorage.Default.SetAsync(AuthTokenKey, tokenDto.Token);
                        Debug.WriteLine($"[AuthService] Login exitoso. Token guardado.");
                        return new MobileLoginResult { IsSuccess = true, TokenData = tokenDto };
                    }
                    else
                    {
                        Debug.WriteLine("[AuthService] Login API OK pero TokenDTO es null o el token está vacío.");
                        return new MobileLoginResult { IsSuccess = false, ErrorMessage = "Respuesta de autenticación inválida del servidor." };
                    }
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"[AuthService] Error en Login API. Status: {response.StatusCode}, Content: {errorContent}");
                    return new MobileLoginResult { IsSuccess = false, ErrorMessage = string.IsNullOrWhiteSpace(errorContent) ? $"Error del servidor: {response.StatusCode}" : errorContent };
                }
            }
            catch (HttpRequestException httpEx)
            {
                Debug.WriteLine($"[AuthService] HttpRequestException en Login: {httpEx.Message}. URL: {ApiBaseUrl}/api/auth/login");
                return new MobileLoginResult { IsSuccess = false, ErrorMessage = "No se pudo conectar al servidor. Verifica tu conexión o la URL de la API." };
            }
            catch (JsonException jsonEx)
            {
                Debug.WriteLine($"[AuthService] JsonException en Login: {jsonEx.Message}");
                return new MobileLoginResult { IsSuccess = false, ErrorMessage = "Error al procesar la respuesta del servidor." };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AuthService] Excepción general en Login: {ex.ToString()}");
                return new MobileLoginResult { IsSuccess = false, ErrorMessage = "Ocurrió un error inesperado durante el inicio de sesión." };
            }
        }

        public async Task LogoutAsync()
        {
            SecureStorage.Default.Remove(AuthTokenKey);
            await Task.CompletedTask;
            Debug.WriteLine("[AuthService] Usuario deslogueado, token eliminado de SecureStorage.");
        }

        public async Task<bool> IsUserAuthenticatedAsync()
        {
            try
            {
                var token = await SecureStorage.Default.GetAsync(AuthTokenKey);
                return !string.IsNullOrEmpty(token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AuthService] Error leyendo token de SecureStorage en IsUserAuthenticatedAsync: {ex.Message}");
                return false;
            }
        }

        public static async Task<string?> GetAuthTokenAsync() // Devuelve string? para indicar que puede ser null
        {
            try
            {
                return await SecureStorage.Default.GetAsync(AuthTokenKey);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AuthService Static] Error obteniendo token (GetAuthTokenAsync): {ex.Message}");
                return null;
            }
        }
    }

    // Asegúrate que IAuthService y MobileLoginResult estén definidos correctamente.
    // Por ejemplo, IAuthService debería tener:
    // Task<MobileLoginResult> LoginAsync(LoginDTO loginRequest);
    // Y MobileLoginResult:
    // public class MobileLoginResult
    // {
    //     public bool IsSuccess { get; set; }
    //     public string? ErrorMessage { get; set; } // Permitir que ErrorMessage sea null
    //     public TokenDTO? TokenData { get; set; } // Permitir que TokenData sea null
    // }
}
