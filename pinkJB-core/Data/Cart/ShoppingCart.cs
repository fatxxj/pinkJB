using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using pinkJB_core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pinkJB_core.Data.Cart
{
    public class ShoppingCart
    {
        public AppDbContext _context { get; set; }
        public string ShoppingCartId { get; set; }
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ShoppingCart(AppDbContext context)
        {
            _context = context;
        }

        public static ShoppingCart GetShoppingCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session; //check if session is null, if yes create
            var context = services.GetService<AppDbContext>();
            string cartId=session.GetString("CartId") ?? Guid.NewGuid().ToString(); //generate new card id
            session.SetString("CartId", cartId);
            return new ShoppingCart(context)
            {
                ShoppingCartId = cartId
            };
        }
        public void AddItemToCart(Product product)
        {
            var shoppingCartItem=_context.ShoppingCartItems.FirstOrDefault(n=>n.Product.Id==product.Id && n.ShoppingCartId==ShoppingCartId);
            if (shoppingCartItem == null) // krijojm item te ri nese nuk e kemi vecse 
            {
                shoppingCartItem = new ShoppingCartItem()
                {
                    ShoppingCartId = ShoppingCartId,
                    Product = product,
                    Amount = 1
                };
                _context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else // nese itemin e kemi ne shopping cart ateher e bejm vec +1 nuk krijohet item i ri
            {
                shoppingCartItem.Amount++;
            }
            _context.SaveChanges();
        }
        public void RemoveItemFromCart(Product product)
        {

            var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault(n => n.Product.Id == product.Id && n.ShoppingCartId == ShoppingCartId);
            if (shoppingCartItem != null) // nese e kemi itemin ne databaz
            {
                if(shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                }else
                {
                    _context.ShoppingCartItems.Remove(shoppingCartItem);
                } 
            }
            
            _context.SaveChanges();

        }
        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ?? (ShoppingCartItems=_context.ShoppingCartItems.Where(n=>n.ShoppingCartId==ShoppingCartId).Include(n=>n.Product).ToList() );
        }
        public double GetShoppingCartTotal()
        {
            var total = _context.ShoppingCartItems.Where(n=>n.ShoppingCartId==ShoppingCartId).Select(n=>n.Product.ProductPrice * n.Amount).Sum();
            return total;
        }

        public async Task ClearShoppingCartAsync()
        {
            var items = await _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).ToListAsync();
            _context.ShoppingCartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }




    }
}
