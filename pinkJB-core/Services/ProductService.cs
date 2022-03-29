using eTickets.Data.Base;
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

        public Task<Product> getProductByIdAsync(int id)
        {
            throw new System.NotImplementedException();

        }

        public Task<Product> getProductByNameAsync(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}
