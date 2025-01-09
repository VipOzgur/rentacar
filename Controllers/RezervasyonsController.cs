﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rentacar.Models;

namespace Rentacar.Controllers
{
    public class RezervasyonsController : Controller
    {
        private readonly DataContext _context;

        public RezervasyonsController()
        {
            _context = new DataContext();
        }

        // GET: Rezervasyons
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Rezervasyons.Include(r => r.Arac).Include(r => r.User);
            return View(await dataContext.ToListAsync());
        }

        // GET: Rezervasyons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervasyon = await _context.Rezervasyons
                .Include(r => r.Arac)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervasyon == null)
            {
                return NotFound();
            }

            return View(rezervasyon);
        }

        // GET: Rezervasyons/Create
        public IActionResult Create()
        {
            ViewData["AracId"] = new SelectList(_context.Araclars, "Id", "Id");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Rezervasyons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AracId,UserId,StartDate,FinishDate,Durum,Onay")] Rezervasyon rezervasyon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rezervasyon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AracId"] = new SelectList(_context.Araclars, "Id", "Id", rezervasyon.AracId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", rezervasyon.UserId);
            return View(rezervasyon);
        }

        // GET: Rezervasyons/Edit/5
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
            ViewData["AracId"] = new SelectList(_context.Araclars, "Id", "Id", rezervasyon.AracId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", rezervasyon.UserId);
            return View(rezervasyon);
        }

        // POST: Rezervasyons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,AracId,UserId,StartDate,FinishDate,Durum,Onay")] Rezervasyon rezervasyon)
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
            ViewData["AracId"] = new SelectList(_context.Araclars, "Id", "Id", rezervasyon.AracId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", rezervasyon.UserId);
            return View(rezervasyon);
        }

        // GET: Rezervasyons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervasyon = await _context.Rezervasyons
                .Include(r => r.Arac)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rezervasyon == null)
            {
                return NotFound();
            }

            return View(rezervasyon);
        }

        // POST: Rezervasyons/Delete/5
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
