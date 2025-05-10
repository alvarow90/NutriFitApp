// Archivo: DTOs/AsignacionDietaDTO.cs
// Ubicación: Proyecto NutriFitApp.Shared
using System; // Necesario para DateTime
using System.ComponentModel.DataAnnotations; // Para anotaciones como [Required]

namespace NutriFitApp.Shared.DTOs
{
    public class AsignacionDietaDTO
    {
        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        public int UsuarioId { get; set; } // ID del usuario al que se asigna la dieta

        // NutriologoId fue eliminado de este DTO.
        // Se obtendrá del token del nutriólogo autenticado en el backend (API).

        [Required(ErrorMessage = "La descripción de la dieta es obligatoria.")]
        [MaxLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha de inicio es obligatoria.")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es obligatoria.")]
        public DateTime FechaFin { get; set; }
    }
}
