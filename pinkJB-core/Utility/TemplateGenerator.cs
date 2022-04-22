using Microsoft.AspNetCore.Identity;
using pinkJB_core.Data.Cart;
using pinkJB_core.Models;
using pinkJB_core.Services;
using System.Text;



namespace pinkJB_core.Utility
{
    public class TemplateGenerator
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProductsService _productsService;
        private readonly ShoppingCart _shoppingCart;
        private readonly IOrdersService _ordersService;

        public TemplateGenerator(IProductsService productsService, ShoppingCart shoppingCart, IOrdersService ordersService, UserManager<ApplicationUser> userManager)
        {
            _productsService = productsService;
            _shoppingCart = shoppingCart;
            _ordersService = ordersService;
            _userManager = userManager;
        }

        public string GetHTMLString()
        {

            var items = _shoppingCart.GetShoppingCartItems();

            var sb = new StringBuilder();
            sb.Append(@"
                        <html>
                            <head>
                            </head>
                            <body>
                                <div class='header'><h1>This is the generated PDF report!!!</h1></div>
                                <table align='center'>
                                    <tr>
                                        <th>Id</th>
                                        <th>Product Name</th>
                                        <th>Amount</th>
                                        <th>ShoppingCartId</th>
                                    </tr>");

            foreach (var item in items)
            {
                sb.AppendFormat(@"<tr>
                                    <td>{0}</td>
                                    <td>{1}</td>
                                    <td>{2}</td>
                                    <td>{3}</td>
                                  </tr>", item.Product.Id, item.Product.ProductName, item.Amount, item.ShoppingCartId);
            }

            sb.Append(@"
                                </table>
                            </body>
                        </html>");

            return sb.ToString();

        }
    }
}
