namespace NutriFitApp.Shared.DTOs
{
    public class TokenDTO
    {
        public string Token { get; set; } = string.Empty;
        public DateTime Expiration { get; set; }
    }
}
