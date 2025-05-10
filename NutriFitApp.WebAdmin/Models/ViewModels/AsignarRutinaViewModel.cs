using System.ComponentModel.DataAnnotations;

namespace NutriFitApp.WebAdmin.ViewModels
{
    public class AsignarRutinaViewModel
    {
        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int EntrenadorId { get; set; }

        [Required]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [Range(1, 60, ErrorMessage = "La duración debe ser entre 1 y 60 días")]
        public int DuracionDias { get; set; }
    }
}
