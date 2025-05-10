namespace NutriFitApp.Shared.DTOs
{
    public class DietaDTO
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
    }
}
