using System.ComponentModel.DataAnnotations;

namespace NutriFitApp.WebAdmin.ViewModels
{
    public class AsignarDietaViewModel
    {
        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int NutriologoId { get; set; }

        [Required]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }
    }
}
