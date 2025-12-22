namespace Boutique.Client.Models
{
    public class LoginResponseDto
    {
        public string? Token { get; set; }
        public DateTime Expiration { get; set; }
        // Add other fields if your API returns more info
    }
}
