using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rentacar.Models;

namespace Rentacar.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly DataContext _context;
        private HelperClass helperClass = new HelperClass();
        public HomeController()
        {
            _context = new DataContext();
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Araclars.ToListAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> Profil()
        {
            var ids = User.FindFirst(type: ClaimTypes.Sid).Value;

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(m => m.Id.ToString() == ids);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        // GET: Users/Edit/5
        public async Task<IActionResult> ProfilEdit()
        {
            var ids = User.FindFirst(type: ClaimTypes.Sid).Value;
            if (ids == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(x=> x.Id.ToString() == ids);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProfilEdit(int id, [FromForm] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string imgPath;
                    if (user.ImageFile != null)
                    {
                        try
                        {
                            imgPath = helperClass.ImageSaveAsWebPAsync(user.ImageFile, user.Ad + user.Soyad); //await
                            user.Profil = imgPath; //Resim yolunu veri tabanýna ekleme
                        }
                        catch (Exception)
                        {
                            _context.DbLogs.Add(new DbLogs
                            {
                                Not = "",
                                Message = "Resim ekleme",
                                Konum = "Home/ProfilEdit"
                            }); ;
                        }
                    }
                    if(user.NewPassword.Any() && user.NewPassword != null)
                    {
                        string passwordHash = helperClass.Hash(user.NewPassword);
                        user.Password = passwordHash;
                    }
                    //edit iþlemi düzenlenecek þifre deðiþtirme eklenecek
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
