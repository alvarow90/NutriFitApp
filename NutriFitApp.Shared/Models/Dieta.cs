using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitApp.Shared.Models
{
    public class Dieta
    {
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [Required]
        public int NutriologoId { get; set; }

        [Required]
        [MaxLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        [ForeignKey("UsuarioId")]
        public User? Usuario { get; set; }

        [ForeignKey("NutriologoId")]
        public User? Nutriologo { get; set; }
    }
}
