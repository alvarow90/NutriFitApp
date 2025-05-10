// Archivo: Services/IAuthService.cs
// Define el contrato para el servicio de autenticación en la aplicación móvil.
using NutriFitApp.Shared.DTOs; // Necesario para LoginDTO y TokenDTO
using System.Threading.Tasks;    // Para Task

namespace NutriFitApp.Mobile.Services
{
    // Clase auxiliar para encapsular el resultado del proceso de login en el móvil.
    // Proporciona información sobre si el login fue exitoso, mensajes de error, y los datos del token.
    public class MobileLoginResult
    {
        public bool IsSuccess { get; set; } // True si el login fue exitoso, false en caso contrario.
        public string ErrorMessage { get; set; } // Mensaje de error si IsSuccess es false.
        public TokenDTO TokenData { get; set; } // Contiene el TokenDTO (token y expiración) devuelto por la API si IsSuccess es true.
    }

    // Interfaz para el servicio de autenticación.
    public interface IAuthService
    {
        // Método para intentar iniciar sesión.
        // Recibe un LoginDTO (email y contraseña) y devuelve un MobileLoginResult.
        Task<MobileLoginResult> LoginAsync(LoginDTO loginRequest);

        // Método para cerrar la sesión del usuario.
        Task LogoutAsync();

        // Método para verificar si el usuario está actualmente autenticado (ej. si hay un token guardado).
        Task<bool> IsUserAuthenticatedAsync();
    }
}
