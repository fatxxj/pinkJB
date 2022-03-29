using Microsoft.EntityFrameworkCore;
using pinkJB_core.Models;

namespace pinkJB_core.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {

        }

        public DbSet<Product> Products { get; set; } //model name
        
    }
}
