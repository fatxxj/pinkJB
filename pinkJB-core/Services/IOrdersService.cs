using pinkJB_core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace pinkJB_core.Services
{
    public interface IOrdersService
    {
        //add orders to DB, get orders from DB
        Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress);
        Task<List<Order>> GetOrdersByUserIdAsync(string userId);


    }
}
