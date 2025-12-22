using System.ComponentModel.DataAnnotations;

namespace WebApiBoutique.Models
{
    // Entity representing garment types (Kurta, Shirt, Pant, Blazer, etc.)
    public class TypeModel
    {
        // Primary key for garment type
        [Key]
        public int TypeId { get; set; }

        // Name of garment type (Kurta, Shirt, Pant, Blazer, Kurti, etc.)
        [Required, StringLength(100)]
        public string TypeName { get; set; } = string.Empty;

        // Navigation property to measurements using this garment type
        public ICollection<Measurement> Measurements { get; set; } = new List<Measurement>();
    }
}
