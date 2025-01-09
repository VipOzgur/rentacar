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
    public class KaskolarsController : Controller
    {
        private readonly DataContext _context;

        public KaskolarsController()
        {
            _context = new DataContext();
        }

        // GET: Kaskolars
        public async Task<IActionResult> Index()
        {
            return View(await _context.Kaskolars.ToListAsync());
        }

        // GET: Kaskolars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kaskolar = await _context.Kaskolars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kaskolar == null)
            {
                return NotFound();
            }

            return View(kaskolar);
        }

        // GET: Kaskolars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kaskolars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ad,GunlukFiyat,SaatlikFiyat")] Kaskolar kaskolar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kaskolar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kaskolar);
        }

        // GET: Kaskolars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kaskolar = await _context.Kaskolars.FindAsync(id);
            if (kaskolar == null)
            {
                return NotFound();
            }
            return View(kaskolar);
        }

        // POST: Kaskolars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad,GunlukFiyat,SaatlikFiyat")] Kaskolar kaskolar)
        {
            if (id != kaskolar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kaskolar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KaskolarExists(kaskolar.Id))
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
            return View(kaskolar);
        }

        // GET: Kaskolars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kaskolar = await _context.Kaskolars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kaskolar == null)
            {
                return NotFound();
            }

            return View(kaskolar);
        }

        // POST: Kaskolars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kaskolar = await _context.Kaskolars.FindAsync(id);
            if (kaskolar != null)
            {
                _context.Kaskolars.Remove(kaskolar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KaskolarExists(int id)
        {
            return _context.Kaskolars.Any(e => e.Id == id);
        }
    }
}
