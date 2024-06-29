using System.ComponentModel.DataAnnotations;

namespace Food_Order.Models
{
    public class CreateProduct
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = "";

        [Required, MaxLength(200)]
        public string Description { get; set; } = "";

        [Required, MaxLength(50)]
        public string Category { get; set; } = "";

        [Required]
        public decimal Price { get; set; }

        public IFormFile? ImageFileName { get; set; }
    }
}
