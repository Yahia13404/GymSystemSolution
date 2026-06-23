using GymSystem.BLL.ViewModels.ApplicationViewModels;
using GymSystem.DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signinManager;

        public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signinManager)
        {
            this.userManager = userManager;
            this.signinManager = signinManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model , CancellationToken ct)
        {
            if (!ModelState.IsValid) 
            {
                return View(model);

            }
            var User = await userManager.FindByEmailAsync(model.Email);
            if (User == null || string.IsNullOrEmpty(User.UserName)) 
            {
                ModelState.AddModelError(string.Empty, "Invalid Email Or Password");
                return View(model);

            }
            var Result = await signinManager.PasswordSignInAsync(User.UserName, model.Password, model.RememberMe ,true );
            if (Result.Succeeded) 
            {
                return RedirectToAction(nameof(HomeController.Index),"Home");
            }

            if (Result.IsLockedOut) 
            {
                ModelState.AddModelError(string.Empty, "This Account Is LockedOut");

            }


            if (Result.IsNotAllowed) 
            {
                ModelState.AddModelError(string.Empty, "Un Authorized");

            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid Email Or Password");
            }
            return View(model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signinManager.SignOutAsync();
            return RedirectToAction(nameof(Login)); 
        }


        public IActionResult AccessDenied() 
        {
            return View();  
        }
    }
}
