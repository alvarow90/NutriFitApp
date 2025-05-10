// Archivo: DTOs/AsignacionRutinaDTO.cs
// Ubicación: Proyecto NutriFitApp.Shared

using System;
using System.ComponentModel.DataAnnotations; // Para anotaciones como [Required]

namespace NutriFitApp.Shared.DTOs
{
    public class AsignacionRutinaDTO
    {
        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        public int UsuarioId { get; set; } // ID del usuario al que se asigna la rutina

        [Required(ErrorMessage = "El nombre de la rutina es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [MaxLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres.")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe especificar los ejercicios.")]
        public string Ejercicios { get; set; } = string.Empty; // Contenido de los ejercicios

        [Required(ErrorMessage = "La duración en días es obligatoria.")]
        [Range(1, 365, ErrorMessage = "La duración debe estar entre 1 y 365 días.")]
        public int DuracionDias { get; set; } // Duración de la rutina en días

        public DateTime? FechaAsignacion { get; set; } // Opcional, la API podría usar DateTime.UtcNow por defecto si es null

        // EntrenadorId NO se incluye aquí, ya que se tomará del token del entrenador autenticado en la API.
    }
}
