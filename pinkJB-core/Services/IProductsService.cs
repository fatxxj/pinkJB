using pinkJB_core.Data.Base;
using pinkJB_core.Data.ViewModels;
using pinkJB_core.Models;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace pinkJB_core.Services
{
    public interface IProductsService : IEntityBaseRepository<Product>
    {
        Task<Product> getByIdAsync(int id);
        Task<Product> getProductByNameAsync(string name);
        //Task<Product> GetAllAsync(params Expression<Func<T, object>>[] includeProperties);
        Task AddNewProductAsync(NewProductVM data);
        Task UpdateProductAsync(NewProductVM data);
    }
}
