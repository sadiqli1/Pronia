using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pronia.Models;
using Pronia.ViewModels;

namespace Pronia.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _usermanager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly Microsoft.AspNetCore.Identity.RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<AppUser> usermanager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _usermanager = usermanager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            if (!ModelState.IsValid) return View();
            if (!register.Terms)
            {
                ModelState.AddModelError("Terms", "Please check our terms and conditions");
                return View();
            }
            
            AppUser user = new AppUser()
            {
                Firstname = register.Firstname,
                Lastname = register.Lastname,
                UserName = register.Username,
                Email = register.Email
            };
            IdentityResult result = await _usermanager.CreateAsync(user, register.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View();
            }

            await _usermanager.AddToRoleAsync(user, "Member");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid) return View();

            AppUser user = await _usermanager.FindByNameAsync(login.Username);
            if (user == null) return View();

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, login.Password, login.Rememberme, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Blocked");
                    return View();
                }
                ModelState.AddModelError("", "Username or Password is incorrect");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult ShowAuthenticated()
        {
            return Json(User.Identity.IsAuthenticated);
        }
        public async Task CreateRole()
        {
            await _roleManager.CreateAsync(new IdentityRole("Member"));
            await _roleManager.CreateAsync(new IdentityRole("Moderator"));
            await _roleManager.CreateAsync(new IdentityRole("Admin"));
        }
    }
}
