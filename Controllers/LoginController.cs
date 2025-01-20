using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Rentacar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Rentacar.Controllers
{
    public class LoginController : Controller
    {
        private readonly DataContext _context;
        private HelperClass helperClass = new HelperClass();

        public LoginController()
        {
            _context = new DataContext();
        }
        [HttpGet]
        public IActionResult Login()
        {
            ClaimsPrincipal claimUser = HttpContext.User;
            if (claimUser.Identity.IsAuthenticated)
            {
                TempData["mesaj"] = $"Ad: {User.FindFirst(ClaimTypes.NameIdentifier).Value} Role: {User.FindFirst(ClaimTypes.Role).Value} Giris yapıldı";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromForm]User p)
        {
            if (p.Ad == null || p.Password == null)
            {
                TempData["hata"] = "Boşlukları Doldurunuz";
                return RedirectToAction("Login", "Login");
            }
            var personel = _context.Users.Include(p => p.Role).FirstOrDefault(x => x.Ad == p.Ad && x.Password == helperClass.Hash(p.Password));
            if (personel != null)
            {
                List<Claim> claims = new List<Claim>() {
                new Claim(ClaimTypes.NameIdentifier,personel.Ad),
                new Claim(ClaimTypes.UserData,(personel.Profil!= null)?personel.Profil:"/imagess/default.png"),
                new Claim(ClaimTypes.Role,(personel.Role!=null)?personel.Role.Ad:"User"),
                new Claim(ClaimTypes.Sid,Convert.ToString(personel.Id)),
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
                TempData["mesaj"] = $"Ad: {personel.Ad} Role: {personel.Role.Ad} Giris yapildi.";
                return RedirectToAction("Index", "Home");
            }
            TempData["hata"] = "Giriþ bilgileriniz yanlış";
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
    }
}
