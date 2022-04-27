using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using pinkJB_core.Models;

namespace pinkJB_core.Data
{
    public class AppDbContext:IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext>options):base(options)
        {

        }

        public DbSet<Product> Products { get; set; } //model namee

        //Order table
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet <ShoppingCartItem> ShoppingCartItems { get; set; }


        public DbSet<FatModel> TestModel { get; set; }


    }
}
