using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NutriFitApp.Shared.Models
{
    public class User : IdentityUser<int>
    {
        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required]
        public string Rol { get; set; } = string.Empty;

        public virtual ICollection<Dieta> Dietas { get; set; } = new List<Dieta>();
        public virtual ICollection<Rutina> Rutinas { get; set; } = new List<Rutina>();
    }
}
