using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pinkJB_core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using pinkJB_core.Services;

namespace pinkJB_core.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductsService _service;

        public HomeController(IProductsService service)
        {
            _service = service;
        }

        
        public IActionResult Index()
        {
            return View();
        }
        public async Task< IActionResult> Details(int id)
        {
            var details = await _service.getByIdAsync(id);
            return View(details);

        }

        
        public async Task<IActionResult> Store()
        {
            var allProducts = await _service.GetAllAsync();
            return View(allProducts);
        }
        public async Task<IActionResult> Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
