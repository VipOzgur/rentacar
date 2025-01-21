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
    public class LokasyonlarsController : Controller
    {
        private readonly DataContext _context;

        public LokasyonlarsController()
        {
            _context = new DataContext();
        }

        // GET: Lokasyonlars
        public async Task<IActionResult> Index()
        {
            return View(await _context.Lokasyonlars.ToListAsync());
        }

        // GET: Lokasyonlars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lokasyonlar = await _context.Lokasyonlars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lokasyonlar == null)
            {
                return NotFound();
            }

            return View(lokasyonlar);
        }

        // GET: Lokasyonlars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Lokasyonlars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ad,Kordinatlar,Durum,Fiyat")] Lokasyonlar lokasyonlar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lokasyonlar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lokasyonlar);
        }

        // GET: Lokasyonlars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lokasyonlar = await _context.Lokasyonlars.FindAsync(id);
            if (lokasyonlar == null)
            {
                return NotFound();
            }
            return View(lokasyonlar);
        }

        // POST: Lokasyonlars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad,Kordinatlar,Durum,Fiyat")] Lokasyonlar lokasyonlar)
        {
            if (id != lokasyonlar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lokasyonlar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LokasyonlarExists(lokasyonlar.Id))
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
            return View(lokasyonlar);
        }

        // GET: Lokasyonlars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var lokasyonlar = await _context.Lokasyonlars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lokasyonlar == null)
            {
                return NotFound();
            }

            return View(lokasyonlar);
        }

        // POST: Lokasyonlars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lokasyonlar = await _context.Lokasyonlars.FindAsync(id);
            if (lokasyonlar != null)
            {
                _context.Lokasyonlars.Remove(lokasyonlar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LokasyonlarExists(int id)
        {
            return _context.Lokasyonlars.Any(e => e.Id == id);
        }
    }
}
