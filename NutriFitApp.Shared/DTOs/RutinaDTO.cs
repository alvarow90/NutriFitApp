// En NutriFitApp.Shared/DTOs/RutinaDTO.cs
namespace NutriFitApp.Shared.DTOs
{
    public class RutinaDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Ejercicios { get; set; } = string.Empty;
        public DateTime? FechaAsignacion { get; set; }
        // Asegúrate de que esta propiedad exista y esté escrita correctamente:
        public int DuracionDias { get; set; } // <--- PROPIEDAD NECESARIA
    }
}