// Archivo: Models/Rutina.cs
// Ubicación: Proyecto NutriFitApp.Shared

using System; // Para DateTime
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Para ForeignKey

namespace NutriFitApp.Shared.Models
{
    public class Rutina
    {
        [Key] // Especifica que Id es la clave primaria (aunque EF Core a menudo lo infiere por convención).
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la rutina es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre de la rutina no puede exceder los 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty; // Nombre de la rutina

        [Required(ErrorMessage = "La descripción de la rutina es obligatoria.")]
        [MaxLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres.")] // Aumentado el MaxLength para descripción
        public string Descripcion { get; set; } = string.Empty; // Descripción general de la rutina

        // Contenido de los ejercicios.
        // Por ahora como string, pero considera refactorizar a una relación con una entidad Ejercicio
        // o una estructura JSON más compleja si necesitas más detalle por ejercicio.
        [Required(ErrorMessage = "Debe especificar los ejercicios de la rutina.")]
        public string Ejercicios { get; set; } = string.Empty;

        [Required(ErrorMessage = "La duración en días es obligatoria.")]
        [Range(1, 365, ErrorMessage = "La duración debe estar entre 1 y 365 días.")]
        public int DuracionDias { get; set; } // Duración de la rutina en días

        public DateTime? FechaAsignacion { get; set; } // Fecha en que se asignó la rutina (nullable si puede no asignarse inmediatamente)

        // --- Claves Foráneas y Propiedades de Navegación ---

        [Required(ErrorMessage = "El ID del usuario es obligatorio.")]
        public int UsuarioId { get; set; } // Clave foránea para el User (cliente)

        [ForeignKey("UsuarioId")] // Especifica que UsuarioId es la FK para la propiedad de navegación Usuario
        public virtual User? Usuario { get; set; } // Propiedad de navegación al usuario (cliente)

        [Required(ErrorMessage = "El ID del entrenador es obligatorio.")]
        public int EntrenadorId { get; set; } // Clave foránea para el User (entrenador)

        [ForeignKey("EntrenadorId")] // Especifica que EntrenadorId es la FK para la propiedad de navegación Entrenador
        public virtual User? Entrenador { get; set; } // Propiedad de navegación al usuario (entrenador)
    }
}
