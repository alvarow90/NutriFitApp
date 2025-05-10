// Archivo: DTOs/RutinaDTO.cs
// Ubicación: Proyecto NutriFitApp.Shared

using System;

namespace NutriFitApp.Shared.DTOs
{
    public class RutinaDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; } // ID del usuario al que se asignó la rutina
        // public int EntrenadorId { get; set; } // Podrías necesitar el ID del entrenador que la asignó

        public string Nombre { get; set; } = string.Empty; // Nombre de la rutina, ej: "Rutina de Fuerza - Día 1"
        public string Descripcion { get; set; } = string.Empty; // Descripción general de la rutina
        public string Ejercicios { get; set; } = string.Empty; // Podría ser un JSON, texto formateado, o necesitarías una lista de DTOs de Ejercicio
        public DateTime? FechaAsignacion { get; set; } // Cuándo se asignó
        // Otros campos que consideres relevantes: Nivel (Principiante, Intermedio, Avanzado), Duración Estimada, etc.

        // Campos que podrías añadir si tu API los provee (requeriría modificar la API para hacer Joins):
        // public string NombreUsuario { get; set; }
        // public string NombreEntrenador { get; set; }
    }
}
