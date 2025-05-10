// Archivo: DTOs/ActualizarUsuarioPerfilDTO.cs
// Ubicación: Proyecto NutriFitApp.Shared

using System;
using System.ComponentModel.DataAnnotations;

namespace NutriFitApp.Shared.DTOs
{
    public class ActualizarUsuarioPerfilDTO
    {
        // Los campos que el usuario puede modificar.
        // El Id y el Email generalmente no se modifican desde este DTO;
        // el Id se obtiene del usuario autenticado y el Email podría tener un proceso de cambio separado.
        // El Rol tampoco se modifica por el usuario mismo.

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El apellido no puede exceder los 100 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        // --- Información Adicional Editable (Opcional, añade los que necesites) ---
        // Asegúrate de que estos campos coincidan con los que quieres que sean editables
        // desde UsuarioPerfilDTO.

        public DateTime? FechaNacimiento { get; set; }

        [Range(1, 300, ErrorMessage = "La altura debe estar entre 1 y 300 cm.")]
        public double? AlturaCm { get; set; } // Altura en centímetros

        [Range(1, 500, ErrorMessage = "El peso debe estar entre 1 y 500 kg.")]
        public double? PesoKg { get; set; } // Peso en kilogramos

        [MaxLength(500, ErrorMessage = "Los objetivos no pueden exceder los 500 caracteres.")]
        public string? Objetivos { get; set; } // Objetivos personales del usuario

        // Ejemplo de otros campos que podrían ser editables:
        // public string? NivelActividad { get; set; }
        // public string? PreferenciasAlimentarias { get; set; }
    }
}
