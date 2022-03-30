using eTickets.Data.Base;
using Microsoft.EntityFrameworkCore;
using pinkJB_core.Data;
using pinkJB_core.Models;
using System.Threading.Tasks;

namespace pinkJB_core.Services
{
    public class ProductService : EntityBaseRepository<Product>, IProductsService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Product> getByIdAsync(int id)
        {
            var movieDetails = await _context.Products
                .FirstOrDefaultAsync(n => n.Id == id);

            return movieDetails;
        }

        

        public Task<Product> getProductByNameAsync(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}
