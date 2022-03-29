using Microsoft.AspNetCore.Mvc;
using pinkJB_core.Models;

namespace pinkJB_core.Controllers
{
    public class StoreController : Controller
    {
        
        public IActionResult Details()
        {
            return View();
        }
        public string Index()
        {
            return "First default action";
        }
        public string Welcome()
        {
            return "This is welcome action method";
        }
    }
}
