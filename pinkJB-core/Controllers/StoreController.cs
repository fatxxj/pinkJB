using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using pinkJB_core.Models;
using pinkJB_core.Services;

namespace pinkJB_core.Controllers
{
    public class StoreController : Controller
    {

        private readonly IProductsService _service;

        public StoreController(IProductsService service)
        {
            _service = service;
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
