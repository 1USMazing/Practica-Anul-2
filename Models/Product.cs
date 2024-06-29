using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Food_Order.Models
{
    public class Product
    { 
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = "";

        [MaxLength(200)]
        public string Description { get; set; } = "";

        [MaxLength(50)]
        public string Category { get; set; } = "";

        [Precision(5, 1)]
        public decimal Price { get; set; }

        [MaxLength(100)]
        public string ImageFileName { get; set; }

        public DateTime CreatedAt { get; set; }




    }




    }

