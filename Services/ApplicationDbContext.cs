using Food_Order.Models;
using Microsoft.EntityFrameworkCore;

namespace Food_Order.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }

    }
}
