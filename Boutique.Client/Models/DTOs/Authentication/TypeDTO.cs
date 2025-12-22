using System.ComponentModel.DataAnnotations;

namespace Boutique.Client.Models.DTOs
{
    public class TypeDto
    {
        public int TypeId { get; set; }

        [Required]
        [StringLength(100)]
        public string TypeName { get; set; } = string.Empty;
    }

    public class CreateTypeDto
    {
        [Required]
        [StringLength(100)]
        public string TypeName { get; set; } = string.Empty;
    }
}
