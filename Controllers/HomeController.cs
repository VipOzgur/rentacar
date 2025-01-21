using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rentacar.Models;

namespace Rentacar.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly DataContext _context;

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
