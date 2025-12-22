// Client-side model representing garment types in the boutique system
// This model mirrors the backend TypeModel for API communication
namespace Boutique.Client.Models
{
    // Data model for garment types (Kurta, Shirt, Pant, Blazer, etc.)
    public class TypeModel
    {
        // Primary key identifier for the garment type
        // Used for database operations and API routing
        public int TypeId { get; set; }

        // Name of the garment type (e.g., "Kurta", "Shirt", "Blazer")
        // Required field for type identification and display
        public string TypeName { get; set; } = string.Empty;
    }
}