using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pinkJB_core.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using pinkJB_core.Services;
using pinkJB_core.Data.ViewModels;

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

        public async Task<IActionResult> Card(int id)
        {
            var item = await _service.getByIdAsync(id);
            return View(item);

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

        public IActionResult Create()
        {
            ViewData["Welcome"] = "Welcome to our store";
            ViewBag.Description = "Store description";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewProductVM product)
        {
            if (!ModelState.IsValid)
            {
                return View(product);
            }
            await _service.AddNewProductAsync(product);

            return RedirectToAction(nameof(Store));
        }
    }
}
