// Archivo: DTOs/UsuarioPerfilDTO.cs
// Ubicación: Proyecto NutriFitApp.Shared

using System;
using System.ComponentModel.DataAnnotations;

namespace NutriFitApp.Shared.DTOs
{
    public class UsuarioPerfilDTO
    {
        // --- Información Básica (generalmente no editable por el usuario directamente aquí) ---
        public int Id { get; set; } // ID del usuario

        [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
        public string Email { get; set; } = string.Empty; // Email del usuario (podría ser solo para visualización)

        public string Rol { get; set; } = string.Empty; // Rol del usuario (para visualización)

        // --- Información Editable del Perfil ---
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El apellido no puede exceder los 100 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        // --- Información Adicional (Opcional, añade los que necesites) ---
        public DateTime? FechaNacimiento { get; set; }

        [Range(1, 300, ErrorMessage = "La altura debe estar entre 1 y 300 cm.")]
        public double? AlturaCm { get; set; } // Altura en centímetros

        [Range(1, 500, ErrorMessage = "El peso debe estar entre 1 y 500 kg.")]
        public double? PesoKg { get; set; } // Peso en kilogramos

        [MaxLength(500, ErrorMessage = "Los objetivos no pueden exceder los 500 caracteres.")]
        public string? Objetivos { get; set; } // Objetivos personales del usuario (ej. perder peso, ganar músculo)

        // Podrías añadir más campos según las necesidades de tu aplicación:
        // public string? NivelActividad { get; set; } // Ej: Sedentario, Ligero, Moderado, Activo, Muy Activo
        // public string? PreferenciasAlimentarias { get; set; }
        // public string? FotoPerfilUrl { get; set; } // Si manejas fotos de perfil
    }
}
