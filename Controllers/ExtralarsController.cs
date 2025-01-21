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
    public class ExtralarsController : Controller
    {
        private readonly DataContext _context;

        public ExtralarsController()
        {
            _context = new DataContext();
        }

        // GET: Extralars
        public async Task<IActionResult> Index()
        {
            return View(await _context.Extralars.ToListAsync());
        }

        // GET: Extralars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var extralar = await _context.Extralars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (extralar == null)
            {
                return NotFound();
            }

            return View(extralar);
        }

        // GET: Extralars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Extralars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Ad,Fiyat,Durum")] Extralar extralar)
        {
            if (ModelState.IsValid)
            {
                _context.Add(extralar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(extralar);
        }

        // GET: Extralars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var extralar = await _context.Extralars.FindAsync(id);
            if (extralar == null)
            {
                return NotFound();
            }
            return View(extralar);
        }

        // POST: Extralars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad,Fiyat,Durum")] Extralar extralar)
        {
            if (id != extralar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(extralar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExtralarExists(extralar.Id))
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
            return View(extralar);
        }

        // GET: Extralars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var extralar = await _context.Extralars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (extralar == null)
            {
                return NotFound();
            }

            return View(extralar);
        }

        // POST: Extralars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var extralar = await _context.Extralars.FindAsync(id);
            if (extralar != null)
            {
                _context.Extralars.Remove(extralar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExtralarExists(int id)
        {
            return _context.Extralars.Any(e => e.Id == id);
        }
    }
}
