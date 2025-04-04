using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rentacar.Models;

namespace Rentacar.Controllers
{
    public class RezervasyonUserController : Controller
    {
        private readonly DataContext _context;
        private HelperClass helperClass = new HelperClass();
        public RezervasyonUserController()
        {
            _context = new DataContext();
        }


        // GET: RezervasyonUser
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            { 
            var dataContext = _context.Rezervasyons.Include(r => r.AlisLokasyon).Include(r => r.Arac).Include(r => r.TeslimLokasyon).Include(r => r.User).Where(x=>x.UserId == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value));
            return View(await dataContext.ToListAsync());
            }
            return NotFound();
        }
        // GET: Araclars
        public async Task<IActionResult> Araclar()
        {
            return View(await _context.Araclars.ToListAsync());
        }

        [HttpPost] //search get json veri
        public IActionResult SearchPost([FromForm] Search search)
        {
            if (search.StartDate == null || search.FinishDate == null || search.FinishDate < search.StartDate)
            {
                TempData["mesaj"] = "Tarih aralığı geçerli değil.";
                var referer = Request.Headers["Referer"].ToString();
                return Redirect(referer);
            }
            var uygunAraclar = _context.Araclars
                    .Where(a => !a.Rezervasyons.Any(r =>
                        (r.StartDate <= search.FinishDate) && (r.FinishDate >= search.StartDate))).ToList();
            ViewBag.search = search; //viewbag tanımı
                return View("Araclar", uygunAraclar);
        }
        [HttpGet] //search get json veri
        public IActionResult SearchGet()
        {
            var lokasyon = _context.Lokasyonlars.Select(x => new {id = x.Id, ad = x.Ad, fiyat = x.Fiyat });
                return Json(lokasyon);
        }

        // GET: RezervasyonUser/AracDetay/5  Araç detay sayfası
        public async Task<IActionResult> AracDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var araclar = await _context.Araclars.Include(r => r.Resims)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (araclar == null)
            {
                return NotFound();
            }

            return View(araclar);
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
        [HttpGet]
        public IActionResult Create(int? aracid,string? today,string? tomorrow)
        {
            var arac =_context.Araclars.Include(r=>r.Resims).FirstOrDefault(r => r.Id == aracid);
            if (arac == null)
                return NotFound();
            ViewData["StartDate"] = today;
            ViewData["FinishDate"] = tomorrow;
            ViewData["Extralar"] = _context.Extralars.ToList();
            ViewData["AlisLokasyonId"] = new SelectList(_context.Lokasyonlars.Where(x=>x.Durum==1).Select(x => new { x.Id, AdFiyat = $"{x.Ad} - {x.Fiyat} ₺" }).ToList(), "Id", "AdFiyat");
            ViewData["Arac"] = arac;
            ViewData["TeslimLokasyonId"] = new SelectList(_context.Lokasyonlars.Where(x => x.Durum == 1).Select(x => new { x.Id, AdFiyat = $"{x.Ad} - {x.Fiyat} ₺" }).ToList(), "Id", "AdFiyat");
            ViewData["User"] = (User.Identity.IsAuthenticated) ? User.FindFirst(ClaimTypes.Sid).Value : -1;
            return View();
        }

        // POST: RezervasyonUser/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] Rezervasyon rezervasyon)
        {
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var user = _context.Users.FirstOrDefault(x => x.Id == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value));
                    rezervasyon.User = user;
                }else
                {
                    rezervasyon.User.Profil = "/imagess/default.png";
                    
                    rezervasyon.User.RoleId = 5;
                    string passwordHash = helperClass.Hash(rezervasyon.User.Password);
                    rezervasyon.User.Password = passwordHash;
                    _context.Add(rezervasyon.User);
                    int changes = await _context.SaveChangesAsync();
                    if (changes == 0)
                    {
                        throw new Exception("Veritabanına kullanıcı kaydedilemedi!");
                    }
                    rezervasyon.UserId= rezervasyon.User.Id;
               }


                // **Ödeme işlemi başlat**
                var paymentResult = await ProcessPayment(rezervasyon);

                if (paymentResult.Status == "success")
                {
                    _context.Add(rezervasyon);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return BadRequest(new { message = "Ödeme başarısız!", error = paymentResult.ErrorMessage });
                }
            }
            ViewData["StartDate"] = rezervasyon.StartDate;
            ViewData["FinishDate"] = rezervasyon.FinishDate;
            ViewData["AlisLokasyonId"] = new SelectList(_context.Lokasyonlars, "Id", "Ad");
            ViewData["Arac"] = rezervasyon.Arac;
            ViewData["TeslimLokasyonId"] = new SelectList(_context.Lokasyonlars, "Id", "Ad");
            ViewData["User"] = (User.Identity.IsAuthenticated) ? User.FindFirst(ClaimTypes.Sid).Value : -1;
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
        public async Task<IActionResult> Edit(int id, [FromForm] Rezervasyon rezervasyon)
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

        /// <summary>
        /// Ödeme işlemi
        /// </summary>
        /// <param name="rezervasyon"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Payment> ProcessPayment(Rezervasyon rezervasyon)
        {
            Options options = new Options
            {
                ApiKey = "sandbox-2FUhsidyfuScSKndWLDqmqIEyLOpckjP",
                SecretKey = "sandbox-JYYuNFbfYNdymlDrNEnGeo6ZlAnpTYnO",
                BaseUrl = "https://sandbox-api.iyzipay.com"
            };

            // Rezervasyon fiyatını hesapla
            var arac = _context.Araclars.FirstOrDefault(a => a.Id == rezervasyon.AracId);
            if (arac == null)
            {
                throw new Exception("Araç bulunamadı!");
            }

            string totalPrice =  rezervasyon.Fiyat.Value.ToString();

            CreatePaymentRequest request = new CreatePaymentRequest
            {
                Locale = Locale.TR.ToString(),
                ConversationId = rezervasyon.Id.ToString(),
                Price = totalPrice,
                PaidPrice = totalPrice,
                Currency = Currency.TRY.ToString(),
                Installment = 1,
                BasketId = "B67832",
                PaymentChannel = PaymentChannel.WEB.ToString(),
                PaymentGroup = PaymentGroup.PRODUCT.ToString(),
                PaymentCard = new PaymentCard
                {
                    CardHolderName = rezervasyon.User.Ad + " " + rezervasyon.User.Soyad,
                    CardNumber = "5528790000000008", // Test kart numarası
                    ExpireMonth = "12",
                    ExpireYear = "2030",
                    Cvc = "123",
                    RegisterCard = 0
                },
                Buyer = new Buyer
                {
                    Id = rezervasyon.UserId.ToString(),
                    Name = rezervasyon.User.Ad,
                    Surname = rezervasyon.User.Soyad,
                    GsmNumber = rezervasyon.User.Telefon,
                    Email = rezervasyon.User.Eposta,
                    IdentityNumber = "74300864791",
                    RegistrationAddress = "Yozgatttttt", //rezervasyon.User.Adres, Kullanıcının adresini tutacağız
                    City = "İstanbul",
                    Country = "Turkey",
                    ZipCode = "34000"
                },
                ShippingAddress = new Address
                {
                    ContactName = rezervasyon.User.Ad + " " + rezervasyon.User.Soyad,
                    City = "Yozgat",
                    Country = "Turkey",
                    ZipCode = "34000",
                    Description = rezervasyon.User.Adres + "sadece bu varsa adres boş"
                },
                BillingAddress = new Address
                {
                    ContactName = rezervasyon.User.Ad + " " + rezervasyon.User.Soyad,
                    City = "İstanbul",
                    ZipCode = "34000",
                    Country = "Turkey",
                    Description = rezervasyon.User.Adres+"sadece bu varsa adres boş"
                },
                BasketItems = new List<BasketItem>
        {
            new BasketItem
            {
                Id = "BI101",
                Name = arac.Model,
                Category1 = "Araç Kiralama",
                ItemType = BasketItemType.VIRTUAL.ToString(),
                Price = totalPrice
            }
        }
            };

            Payment payment = await Payment.Create(request, options);
            return payment;
        }

    }
}
