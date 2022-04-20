using Microsoft.EntityFrameworkCore;
using pinkJB_core.Data;
using pinkJB_core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pinkJB_core.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly AppDbContext _context; //inject appdb context kur punojm me informata prej databaze
        public OrdersService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId,string userRole)
        {
            var orders = await _context.Orders.Include(n => n.OrderItems).ThenInclude(n => n.Product).Include(n => n.User).ToListAsync();
            if (userRole!="ADMIN")
            {
                orders = orders.Where(n => n.userId == userId).ToList();
            }
            return orders;
        }

        public async Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress)
        {
            var order = new Order()
            {
                userId = userId,
                Email = userEmailAddress
            };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            foreach (var item in items)
            {
                var orderItem = new OrderItem()
                {
                    Amount = item.Amount,
                    ProductId = item.Product.Id,
                    OrderId = order.Id,
                    Price = item.Product.ProductPrice
                };
                await _context.OrderItems.AddAsync(orderItem);
               
            }
            await _context.SaveChangesAsync();
        }
    }
}
