namespace NutriFitApp.Shared.DTOs
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public int? NutriologoId { get; set; }
        public int? EntrenadorId { get; set; }
    }
}
