using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pinkJB_core.Data;
using pinkJB_core.Data.ViewModels;
using pinkJB_core.Models;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace pinkJB_core.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        
        public IActionResult Login()
        {
            var response = new LoginVM();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if(!ModelState.IsValid)
                return View(loginVM);

            //check user exists in DB
            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if(user!=null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if(passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false,false);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Store", "Home");
                    }
                }
                TempData["Error"] = "Password or Email address is incorrect.";
                return View(loginVM);
            }
            TempData["Error"] = "Password or Email address is incorrect.";
            return View(loginVM);

        }
    }
}
