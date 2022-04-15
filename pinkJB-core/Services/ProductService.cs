using eTickets.Data.Base;
using Microsoft.EntityFrameworkCore;
using pinkJB_core.Data;
using pinkJB_core.Data.ViewModels;
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

        public async Task AddNewProductAsync(NewProductVM data)
        {
            var newProduct = new Product()
            {
                ProductName = data.ProductName,
                ProductDescription = data.ProductDescription,
                ProductPrice = data.ProductPrice,
                ProductImage = data.ProductImage,
                amountLeft = data.amountLeft
                
            };
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            

        }

       

        public async Task UpdateProductAsync(NewProductVM data)
        {

            var dbProduct = await _context.Products.FirstOrDefaultAsync(n=>n.Id == data.Id);
            if(dbProduct!=null)
            {

                dbProduct.ProductName = data.ProductName;
                dbProduct.ProductDescription = data.ProductDescription;
                dbProduct.ProductPrice = data.ProductPrice;
                dbProduct.ProductImage = data.ProductImage;
                dbProduct.ProductMaterial = data.ProductMaterial;
                    dbProduct.amountLeft = data.amountLeft;
                await _context.SaveChangesAsync();

            }

           
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
