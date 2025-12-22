namespace Boutique.Client.Models.DTOs.Authentication
{
    public class PasswordHistoryDTO
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public DateTime ChangedAt { get; set; }
    }
}