// Archivo: Services/IDietaService.cs
// Ubicación: Proyecto NutriFitApp.Mobile
using NutriFitApp.Shared.DTOs; // Para DietaDTO
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NutriFitApp.Mobile.Services
{
    public interface IDietaService
    {
        // Método actualizado para obtener las dietas específicas del usuario autenticado.
        Task<List<DietaDTO>> GetMisDietasAsync();
    }
}
