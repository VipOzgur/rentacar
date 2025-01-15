using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using Rentacar.Models;

namespace Rentacar.Controllers
{
    public class AraclarsController : Controller
    {
        private readonly DataContext _context;
        private HelperClass helperClass = new HelperClass();
        private  string imgPath = "";

        public AraclarsController()
        {
            _context = new DataContext();
        }

        // GET: Araclars
        public async Task<IActionResult> Index()
        {
            return View(await _context.Araclars.ToListAsync());
        }

        // GET: Araclars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var araclar = await _context.Araclars.Include(r=>r.Resims)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (araclar == null)
            {
                return NotFound();
            }

            return View(araclar);
        }

        // GET: Araclars/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Araclars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Araclar araclar)
        {
            if (ModelState.IsValid)
            {
                
                if (araclar.ImageFile != null)
                {
                    try
                    {
                        imgPath = await helperClass.ImageSaveAsWebPAsync(araclar.ImageFile);
                        araclar.Profil = imgPath; //Resim yolunu veri tabanına ekleme
                    }
                    catch (Exception)
                    {
                        araclar.Profil = "/imagess/default.png";
                        _context.DbLogs.Add(new DbLogs
                        {
                            Not = "",
                            Message = "Resim ekleme",
                            Konum= "Araclars Create"
                        }); ;
                    }
                }
                else
                {
                    //değil ise varsayılanı ekliyoruz
                    araclar.Profil = "/imagess/default.png";
                }
                if (araclar.ImageFiles != null) {
                    try
                    {
                        foreach (var item in araclar.ImageFiles)
                        {
                            imgPath = await helperClass.ImageSaveAsWebPAsync(item);
                            araclar.Resims.Add(new Resim { Url=imgPath, AltG="Araç Resmi"});
                        }
                    }
                    catch (Exception)
                    {
                        _context.DbLogs.Add(new DbLogs
                        {
                            Not = "",
                            Message = "Resim ekleme",
                            Konum = "Araclars Create"
                        });
                    }
                }
                _context.Add(araclar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(araclar);
        }

        // GET: Araclars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var araclar = await _context.Araclars.FindAsync(id);
            if (araclar == null)
            {
                return NotFound();
            }
            return View(araclar);
        }

        // POST: Araclars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Aciklama,Plaka,Marka,Model,IsActive,Profil,Yakit,Vites,Fiyat,Durum,Depozito,Km,GoruntulemeSayisi,KiralanmaSayisi,MotorGucu,YakitKm")] Araclar araclar)
        {
            if (id != araclar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(araclar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AraclarExists(araclar.Id))
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
            return View(araclar);
        }

        // GET: Araclars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var araclar = await _context.Araclars
                .FirstOrDefaultAsync(m => m.Id == id);
            if (araclar == null)
            {
                return NotFound();
            }

            return View(araclar);
        }

        // POST: Araclars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var araclar = await _context.Araclars.FindAsync(id);
            if (araclar != null)
            {
                _context.Araclars.Remove(araclar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AraclarExists(int id)
        {
            return _context.Araclars.Any(e => e.Id == id);
        }
    }
}
