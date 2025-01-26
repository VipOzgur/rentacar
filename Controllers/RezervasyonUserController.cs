using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rentacar.Models;

namespace Rentacar.Controllers
{
    public class RezervasyonUserController : Controller
    {
        private readonly DataContext _context;

        public RezervasyonUserController()
        {
            _context = new DataContext();
        }


        // GET: RezervasyonUser
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Rezervasyons.Include(r => r.AlisLokasyon).Include(r => r.Arac).Include(r => r.TeslimLokasyon).Include(r => r.User);
            return View(await dataContext.ToListAsync());
        }
        // GET: Araclars
        public async Task<IActionResult> Araclar()
        {
            return View(await _context.Araclars.ToListAsync());
        }

        [HttpGet] //search get json veri
        public IActionResult SearchPost(DateTime startDate, DateTime finishDate)
        {
                var availableCars = _context.Araclars
                    .Where(a => !a.Rezervasyons.Any(r =>
                        (r.StartDate <= finishDate) && (r.FinishDate >= startDate))).ToList();

                return Json(availableCars);
        }
        [HttpGet] //search get json veri
        public IActionResult SearchGet()
        {
            var lokasyon = _context.Lokasyonlars.Select(x => new { Id = x.Id, Name = x.Ad, Price = x.Fiyat });
                return Json(lokasyon);
        }

        // GET: RezervasyonUser/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervasyon = await _context.Rezervasyons
                .Include(r => r.AlisLokasyon)
                .Include(r => r.Arac)
                .Include(r => r.TeslimLokasyon)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervasyon == null)
            {
                return NotFound();
            }

            return View(rezervasyon);
        }

        // GET: RezervasyonUser/Create
        public IActionResult Create()
        {
            ViewData["AlisLokasyonId"] = new SelectList(_context.Lokasyonlars, "Id", "Ad");
            ViewData["AracId"] = new SelectList(_context.Araclars, "Id", "Ad");
            ViewData["TeslimLokasyonId"] = new SelectList(_context.Lokasyonlars, "Id", "Ad");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Ad");
            return View();
        }

        // POST: RezervasyonUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AracId,UserId,StartDate,FinishDate,Durum,Onay,AlisLokasyonId,TeslimLokasyonId,KaskoId,Fiyat,Not,Id,CreateDate,UpdateDate")] Rezervasyon rezervasyon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rezervasyon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AlisLokasyonId"] = new SelectList(_context.Lokasyonlars, "Id", "Ad", rezervasyon.AlisLokasyonId);
            ViewData["AracId"] = new SelectList(_context.Araclars, "Id", "Ad", rezervasyon.AracId);
            ViewData["TeslimLokasyonId"] = new SelectList(_context.Lokasyonlars, "Id", "Ad", rezervasyon.TeslimLokasyonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Ad", rezervasyon.UserId);
            return View(rezervasyon);
        }

        // GET: RezervasyonUser/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervasyon = await _context.Rezervasyons.FindAsync(id);
            if (rezervasyon == null)
            {
                return NotFound();
            }
            ViewData["AlisLokasyonId"] = new SelectList(_context.Lokasyonlars, "Id", "Ad", rezervasyon.AlisLokasyonId);
            ViewData["AracId"] = new SelectList(_context.Araclars, "Id", "Ad", rezervasyon.AracId);
            ViewData["TeslimLokasyonId"] = new SelectList(_context.Lokasyonlars, "Id", "Ad", rezervasyon.TeslimLokasyonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Ad", rezervasyon.UserId);
            return View(rezervasyon);
        }

        // POST: RezervasyonUser/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AracId,UserId,StartDate,FinishDate,Durum,Onay,AlisLokasyonId,TeslimLokasyonId,KaskoId,Fiyat,Not,Id,CreateDate,UpdateDate")] Rezervasyon rezervasyon)
        {
            if (id != rezervasyon.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervasyon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervasyonExists(rezervasyon.Id))
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
            ViewData["AlisLokasyonId"] = new SelectList(_context.Lokasyonlars, "Id", "Ad", rezervasyon.AlisLokasyonId);
            ViewData["AracId"] = new SelectList(_context.Araclars, "Id", "Ad", rezervasyon.AracId);
            ViewData["TeslimLokasyonId"] = new SelectList(_context.Lokasyonlars, "Id", "Ad", rezervasyon.TeslimLokasyonId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Ad", rezervasyon.UserId);
            return View(rezervasyon);
        }

        // GET: RezervasyonUser/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervasyon = await _context.Rezervasyons
                .Include(r => r.AlisLokasyon)
                .Include(r => r.Arac)
                .Include(r => r.TeslimLokasyon)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervasyon == null)
            {
                return NotFound();
            }

            return View(rezervasyon);
        }

        // POST: RezervasyonUser/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervasyon = await _context.Rezervasyons.FindAsync(id);
            if (rezervasyon != null)
            {
                _context.Rezervasyons.Remove(rezervasyon);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RezervasyonExists(int id)
        {
            return _context.Rezervasyons.Any(e => e.Id == id);
        }
    }
}
