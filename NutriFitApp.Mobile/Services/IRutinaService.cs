// Archivo: Services/IRutinaService.cs
// Ubicación: Proyecto NutriFitApp.Mobile
using NutriFitApp.Shared.DTOs; // Para RutinaDTO
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NutriFitApp.Mobile.Services
{
    public interface IRutinaService
    {
        // Método para obtener las rutinas específicas del usuario autenticado.
        Task<List<RutinaDTO>> GetMisRutinasAsync();

        // Podrías añadir más métodos aquí en el futuro, como:
        // Task<RutinaDTO> GetRutinaByIdAsync(int rutinaId);
        // Task<bool> MarcarEjercicioComoCompletoAsync(int rutinaId, int ejercicioId);
    }
}
