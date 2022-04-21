using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using pinkJB_core.Data;
using pinkJB_core.Data.Static;
using pinkJB_core.Data.ViewModels;
using pinkJB_core.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace pinkJB_core.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly ILogger<AccountController> _logger;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            
        }

        public async Task<IActionResult> Users()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        public IActionResult Login()
        {
            var response = new LoginVM();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
                return View(loginVM);

            //check user exists in DB
            var user = await _userManager.FindByEmailAsync(loginVM.EmailAddress);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
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

        public IActionResult Register() => View(new RegisterVM());

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
                return View(registerVM);

            var user = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
            if (user != null)
            {
                TempData["Error"] = "This email address is already in use.";
                return View(registerVM);
            }
            var newUser = new ApplicationUser()
            {
                FullName = registerVM.FullName,
                Email = registerVM.EmailAddress,
                UserName = registerVM.EmailAddress,
            };
            var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Password);
            if (newUserResponse.Succeeded)
            {
                
               

                await _userManager.AddToRoleAsync(newUser, UserRoles.User);
                MailMessage mm = new MailMessage();
                mm.Subject = "Account registered";
                mm.Body = "You account is registered successfully! You can log in using your credentials. Thank you for being a part of us.  ";
                mm.IsBodyHtml = false;
                mm.From = new MailAddress("pink.jb10@gmail.com", "PinkJB admin");
                mm.To.Add(new MailAddress(registerVM.EmailAddress, "Again fat"));
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("pink.jb10@gmail.com", "Pinkjb@1234");
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);

            }
            return View("RegisterCompleted");
        }

        //this method registers new admin only when you are already signed in as admin

        [HttpPost]
        public async Task<IActionResult> RegisterAdmin(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
                return View(registerVM);

            var adminUser = await _userManager.FindByEmailAsync(registerVM.EmailAddress);
           // var adminUser = await _userManager.FindByEmailAsync(adminUserEmail);
            if (adminUser == null)
            {
                var newAdminUser = new ApplicationUser()
                {
                    FullName = registerVM.FullName,
                    UserName = registerVM.EmailAddress,
                    Email = registerVM.EmailAddress,
                    EmailConfirmed = true


                };
                await _userManager.CreateAsync(newAdminUser, registerVM.Password);
                await _userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                await _context.SaveChangesAsync();

            }

            //var newUserResponse = await _userManager.CreateAsync(adminUser, registerVM.Password);
            //if (newUserResponse.Succeeded)
            //{
           //     await _userManager.AddToRoleAsync(adminUser, UserRoles.Admin);
//
          //  }
            return View("RegisterCompleted");

        }



        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Store", "Home");

        }

        /*
        public IActionResult SendGmailSuccessfulRegistration()
        {
            MailMessage mm = new MailMessage();
            mm.Subject = "Hello user";
            mm.Body = "This is a test ";
            mm.IsBodyHtml = false;
            mm.From = new MailAddress("pink.jb10@gmail.com", "from Fat");
            mm.To.Add(new MailAddress("halimifat@gmail.com", "Again fat"));
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            NetworkCredential NetworkCred = new NetworkCredential("pink.jb10@gmail.com", "Pinkjb@1234");
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = NetworkCred;
            smtp.Port = 587;
            smtp.Send(mm);
            return RedirectToAction("Store", "Home");
        }

        */
    }
}
