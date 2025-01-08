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
    public class ResimsController : Controller
    {
        private readonly DataContext _context;

        public ResimsController()
        {
            _context = new DataContext();
        }

        // GET: Resims
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Resims.Include(r => r.Arac);
            return View(await dataContext.ToListAsync());
        }

        // GET: Resims/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resim = await _context.Resims
                .Include(r => r.Arac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resim == null)
            {
                return NotFound();
            }

            return View(resim);
        }

        // GET: Resims/Create
        public IActionResult Create()
        {
            ViewData["AracId"] = new SelectList(_context.Araclars, "Id", "Id");
            return View();
        }

        // POST: Resims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AracId,Url,AltG")] Resim resim)
        {
            if (ModelState.IsValid)
            {
                _context.Add(resim);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AracId"] = new SelectList(_context.Araclars, "Id", "Id", resim.AracId);
            return View(resim);
        }

        // GET: Resims/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resim = await _context.Resims.FindAsync(id);
            if (resim == null)
            {
                return NotFound();
            }
            ViewData["AracId"] = new SelectList(_context.Araclars, "Id", "Id", resim.AracId);
            return View(resim);
        }

        // POST: Resims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AracId,Url,AltG")] Resim resim)
        {
            if (id != resim.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(resim);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResimExists(resim.Id))
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
            ViewData["AracId"] = new SelectList(_context.Araclars, "Id", "Id", resim.AracId);
            return View(resim);
        }

        // GET: Resims/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var resim = await _context.Resims
                .Include(r => r.Arac)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (resim == null)
            {
                return NotFound();
            }

            return View(resim);
        }

        // POST: Resims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var resim = await _context.Resims.FindAsync(id);
            if (resim != null)
            {
                _context.Resims.Remove(resim);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResimExists(int id)
        {
            return _context.Resims.Any(e => e.Id == id);
        }
    }
}
