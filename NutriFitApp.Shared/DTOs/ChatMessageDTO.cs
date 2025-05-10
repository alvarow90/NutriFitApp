namespace NutriFitApp.Shared.DTOs
{
    public class ChatMessageDTO
    {
        public int Id { get; set; }
        public int EmisorId { get; set; }
        public int ReceptorId { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
    }
}
