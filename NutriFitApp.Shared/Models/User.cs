// Archivo: Models/User.cs
// Ubicación: Proyecto NutriFitApp.Shared

using Microsoft.AspNetCore.Identity;
using System; // Para DateTime
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations; // Para MaxLength, etc.

namespace NutriFitApp.Shared.Models
{
    public class User : IdentityUser<int> // Asumiendo que tu clave primaria es int
    {
        // --- Propiedades existentes ---
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El apellido no puede exceder los 100 caracteres.")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El rol es obligatorio.")] // Considera si este campo es redundante con los roles de Identity
        public string Rol { get; set; } = string.Empty; // Rol principal o específico de la app

        // --- Propiedades de navegación existentes ---
        public virtual ICollection<Dieta> Dietas { get; set; } = new List<Dieta>();
        public virtual ICollection<Rutina> Rutinas { get; set; } = new List<Rutina>();

        // --- NUEVAS PROPIEDADES PARA EL PERFIL DE USUARIO ---
        public DateTime? FechaNacimiento { get; set; }

        [Range(30, 300, ErrorMessage = "La altura debe estar entre 30 y 300 cm.")]
        public double? AlturaCm { get; set; } // Altura en centímetros

        [Range(1, 700, ErrorMessage = "El peso debe estar entre 1 y 700 kg.")]
        public double? PesoKg { get; set; } // Peso en kilogramos

        [MaxLength(1000, ErrorMessage = "Los objetivos no pueden exceder los 1000 caracteres.")]
        public string? Objetivos { get; set; } // Objetivos personales del usuario

        // Otros campos que podrías considerar para el perfil:
        // public string? Genero { get; set; }
        // public string? NivelActividadFisica { get; set; } // Ej: Sedentario, Ligero, Moderado, Activo
        // public string? FotoPerfilUrl { get; set; }
    }
}
