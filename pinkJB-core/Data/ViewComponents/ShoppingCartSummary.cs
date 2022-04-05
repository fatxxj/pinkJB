using Microsoft.AspNetCore.Mvc;
using pinkJB_core.Data.Cart;

namespace pinkJB_core.Data.ViewComponents
{
    
    public class ShoppingCartSummary:ViewComponent
    {
        private readonly ShoppingCart _shoppingCart;
        public ShoppingCartSummary(ShoppingCart shoppingCart)
        {
            _shoppingCart = shoppingCart;
        }
        public IViewComponentResult Invoke()   // in controllers we have i action result here we have viewCompoenentResult
        {
            var items=_shoppingCart.GetShoppingCartItems();
            return View(items.Count);
        }
    }
}
