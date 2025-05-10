// Archivo: Services/IPerfilService.cs
// Ubicación: Proyecto NutriFitApp.Mobile
using NutriFitApp.Shared.DTOs; // Para UsuarioPerfilDTO y ActualizarUsuarioPerfilDTO
using System.Threading.Tasks;

namespace NutriFitApp.Mobile.Services
{
    // Clase para encapsular el resultado de la operación de actualizar perfil
    public class ActualizarPerfilResult
    {
        public bool IsSuccess { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public interface IPerfilService
    {
        // Método para obtener el perfil del usuario actualmente autenticado
        Task<UsuarioPerfilDTO?> GetMiPerfilAsync();

        // Método para actualizar el perfil del usuario actualmente autenticado
        Task<ActualizarPerfilResult> ActualizarMiPerfilAsync(ActualizarUsuarioPerfilDTO perfilDto);
    }
}
