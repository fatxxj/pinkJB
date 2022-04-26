using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pinkJB_core.Data.Cart;
using pinkJB_core.Data.ViewModels;
using pinkJB_core.Models;
using pinkJB_core.Services;
using pinkJB_core.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace pinkJB_core.Controllers
{
    public class OrdersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IProductsService _productsService;
        private readonly ShoppingCart _shoppingCart;
        private readonly IOrdersService _ordersService;
        private readonly IConverter _converter;

        public OrdersController(IProductsService productsService, ShoppingCart shoppingCart, IOrdersService ordersService,  UserManager<ApplicationUser> userManager, IConverter converter)
        {
            _productsService = productsService;
            _shoppingCart = shoppingCart;
            _ordersService = ordersService;
            _userManager = userManager;
            _converter= converter;
        }
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var orders = await _ordersService.GetOrdersByUserIdAndRoleAsync(userId, userRole);
            return View(orders);

        }
        public IActionResult ShoppingCart()
        {
            var items = _shoppingCart.GetShoppingCartItems();//return list of shopping cart item
            _shoppingCart.ShoppingCartItems = items;
            var response = new ShoppingCartVM()
            {
                ShoppingCart = _shoppingCart,
                ShoppingCartTotal = _shoppingCart.GetShoppingCartTotal()
            };
            return View(response);
        }
        public async Task<IActionResult> AddToShoppingCart(int id)
        {
            var item = await _productsService.GetByIdAsync(id);
            if(item!=null)
            {
                _shoppingCart.AddItemToCart(item);
            }
            return RedirectToAction("Store","Home");
        }
        public async Task<IActionResult> AddToShoppingCart1(int id)
        {
            var item = await _productsService.GetByIdAsync(id);
            if (item != null)
            {
                _shoppingCart.AddItemToCart(item);
            }
            return RedirectToAction("ShoppingCart", "Orders");
        }


        public async Task<IActionResult> RemoveFromShoppingCart(int id)
        {
            var item = await _productsService.GetByIdAsync(id);
            if (item != null)
            {
                _shoppingCart.RemoveItemFromCart(item);
            }
            if (item == null)
            {
                return RedirectToAction("Store", "Home");

            }

            return RedirectToAction(nameof(ShoppingCart));
        }

        /*
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
        */

        public async Task<IActionResult> CompleteOrder()
        {
            
            var items=_shoppingCart.GetShoppingCartItems();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            string userEmailAddress = User.FindFirstValue(ClaimTypes.Email); 
            await _ordersService.StoreOrderAsync(items,userId,userEmailAddress);
            await _shoppingCart.ClearShoppingCartAsync();
            double total1 = 0;
            Random random = new Random();
            var num = random.Next();
            var message = "";
            foreach (var item in items)
            {
                
                var total = item.Amount * item.Product.ProductPrice;
                message += "  You have successfuly ordered  " + item.Product.ProductName + " whith a  price of " + total.ToString("C", new CultureInfo("en-US")) + "<br><br>" ;
                 total1 = total1 + total;

            }
            
            message += " The grand total is: " + total1.ToString("C", new CultureInfo("en-US"));

            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = "PDF Report",
                Out = @"C:\Users\fatha\Desktop\Faktura"+ num + ".pdf"  //USE THIS PROPERTY TO SAVE PDF TO A PROVIDED LOCATION
            };


            var message1 = "  <table class='table customFont1' > <tr> <th scope='col'> Product Name </th> <th scope='col'> Amount </th> <th scope='col'>Product Price</th> <th scope='col'>Total</th> </tr> ";
            total1 = 0;
            foreach (var item in items)
            {


                var total = item.Amount * item.Product.ProductPrice;
                // message1 += "  You have successfuly ordered  " + item.Product.ProductName + " whith a  price of " + total + "<br><br>";
                total1 = total1 + total;
                message1 += "<tr class='customFont1' style='18px'> <td class='align-middle' style='22px'>" + item.Product.ProductName + "</td> <td class='align-middle'>" + item.Amount + "</td> <td class='align-middle'>" + item.Product.ProductPrice.ToString("C", new CultureInfo("en-US")) + "</td> <td class='align-middle'>" + total.ToString("C", new CultureInfo("en-US")) + "</td>  </tr> ";

            }


            message1 += "</table>";
            message1 += "<h4>The grand total is: " + total1.ToString("C", new CultureInfo("en-US")) + "</h4>";
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = "<br><br><h1 class= 'customFont text-center'>Order's receipt</h1>" + "<br><br>" +
                 message1 + "<br>"
                 + "<h4>Your shopping cart id is: " + _shoppingCart.ShoppingCartId + "<br>" + "<footer>If you dont recognize this, contact us!</h4>"



               ,

                //Page = "https://localhost:44397/Orders/ShoppingCart", //USE THIS PROPERTY TO GENERATE PDF CONTENT FROM AN HTML PAGE
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "bootstrap.css") },
               // HeaderSettings = { FontName = "Arial", FontSize = 16, Right = "Page [page] of [toPage]", Line = true },
                //FooterSettings = { FontName = "Arial", FontSize = 16, Line = true, Center = "Report Footer" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };

            //_converter.Convert(pdf); IF WE USE Out PROPERTY IN THE GlobalSettings CLASS, THIS IS ENOUGH FOR CONVERSION

            var file = _converter.Convert(pdf);


            MailMessage mm = new MailMessage();
            mm.Subject = "Successful order";
            mm.Body = message;
            mm.IsBodyHtml = true;
            mm.From = new MailAddress("pink.jb10@gmail.com", "Pink JB admin");
            mm.To.Add(new MailAddress(userEmailAddress, "Fat Halimi"));
            mm.Attachments.Add(new Attachment(@"C:\Users\fatha\Desktop\Faktura"+ num + ".pdf"));

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential("pink.jb10@gmail.com", "Pinkjb@1234");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.Send(mm);


            MailMessage mailAdmin = new MailMessage();
            mailAdmin.Subject = "Successful order made by: "+userEmailAddress;
            mailAdmin.Body = message;
            mailAdmin.IsBodyHtml = true;
            mailAdmin.From = new MailAddress("pink.jb10@gmail.com", "Pink JB admin");
            mailAdmin.To.Add(new MailAddress("pink.jb10@gmail.com", "PinkJB"));
            mailAdmin.Attachments.Add(new Attachment(@"C:\Users\fatha\Desktop\Faktura" + num + ".pdf"));

            smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
             NetworkCred = new NetworkCredential("pink.jb10@gmail.com", "Pinkjb@1234");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.Send(mailAdmin);


           

            //return Ok("Successfully created PDF document.");
            //return File(file, "application/pdf", "EmployeeReport.pdf");
            //return File(file, "application/pdf");





            return View("OrderCompleted");
        }


    }
}
