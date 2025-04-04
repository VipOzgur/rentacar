using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Rentacar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

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

                return RedirectToAction("Index", "Admin");
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
                new Claim(ClaimTypes.Name,personel.Ad),
                new Claim(ClaimTypes.Surname,personel.Soyad),
                new Claim(ClaimTypes.MobilePhone,personel.Telefon),
                new Claim(ClaimTypes.Email,personel.Eposta),
                new Claim(ClaimTypes.UserData,(personel.Profil!= null)?personel.Profil:"/imagess/default.png"),
                new Claim(ClaimTypes.Role,(personel.Role!=null)?personel.Role.Ad:"user"),
                new Claim(ClaimTypes.Sid,Convert.ToString(personel.Id)),
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties = new AuthenticationProperties()
                {
                    AllowRefresh = true
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
                TempData["mesaj"] = $"Ad: {personel.Ad} Role: {personel.Role.Ad} Giris yapildi.";
                if (personel.Role ==null || personel.Role.Ad=="user")
                {
                return RedirectToAction("Index", "Home");
                }
                return RedirectToAction("Index", "Admin");
            }
            TempData["hata"] = "Giriþ bilgileriniz yanlış";
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register([FromForm] User user)
        {
            if (ModelState.IsValid)
            {
                string imgPath = "";
                if (user.ImageFile != null)
                {
                    try
                    {
                        imgPath = helperClass.ImageSaveAsWebPAsync(user.ImageFile, user.Ad + user.Soyad); //await
                        user.Profil = imgPath; //Resim yolunu veri tabanına ekleme
                    }
                    catch (Exception)
                    {
                        user.Profil = "/imagess/default.png";
                        _context.DbLogs.Add(new DbLogs
                        {
                            Not = "",
                            Message = "Resim ekleme",
                            Konum = "Araclars Create"
                        }); ;
                    }
                }
                else
                {
                    //değil ise varsayılanı ekliyoruz
                    user.Profil = "/imagess/default.png";
                }
                user.RoleId = 5;
                string passwordHash = helperClass.Hash(user.Password);
                user.Password = passwordHash;
                _context.Add(user);
                _context.SaveChangesAsync();
                return RedirectToAction("Login", "Login",user); // Login get methoduna gidiyor çözüm clamleri ayrı methodda oluşturmak
            }
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Ad", user.RoleId);
            return View(user);
        }

        [AllowAnonymous]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Login");
        }
    }
}
