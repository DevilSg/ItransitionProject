using ItransitionProject.Models;
using ItransitionProject.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ItransitionProject.Controllers
{
    public class AccountController : Controller
    {
        private SiteReviewContext _context;

        public AccountController(SiteReviewContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
           
            return View();
        }
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model) 
        {
            if (ModelState.IsValid)
            {
                User? user = await _context.Users.Include(u=>u.FkroleNavigation).FirstOrDefaultAsync(u => u.UserName == model.UserName && u.UserPassword == model.UserPassword);
                foreach (var s in _context.Users.ToList())
                {
                    if (user?.UserName == s.UserName && user?.UserPassword == s.UserPassword)
                    {
                        await Authenticate(user);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError(string.Empty, "incorrect username and(or) password");
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User? user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == model.UserName);
                if (user == null)
                {
                    
                    user = new User { UserName = model.UserName, UserPassword = model.UserPassword };
                   
                    RoleUser? userRole = await _context.RoleUsers.FirstOrDefaultAsync(r => r.RoleName == "user");
                    if (userRole != null)
                        user.FkroleNavigation = userRole;

                    _context.Users.Add(user);
                    await _context.SaveChangesAsync();

                    await Authenticate(user); 

                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError(string.Empty, "incorrect username and(or) password");
            }
            return View(model);
        }

        private async Task Authenticate(User user)
        {
            
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.FkroleNavigation.RoleName)
            };
            
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            
            
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("~/Home");
        }
    }
}
