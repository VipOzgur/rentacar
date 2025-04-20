using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Rentacar.Models;
using Rentacar.Services;
using System.Security.Claims;
using System.Text.Json;

namespace Rentacar.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PaymentService _paymentService;
        private readonly DataContext _context;
        private HelperClass helperClass = new HelperClass();

        public PaymentController(PaymentService paymentService)
        {
            _context = new DataContext();
            _paymentService = paymentService;
        }

        // GET: RezervasyonUser/Create
        [HttpGet]
        public IActionResult Checkout(int? aracid, string? today, string? tomorrow)
        {
            var arac = _context.Araclars.Include(r => r.Resims).FirstOrDefault(r => r.Id == aracid);
            if (arac == null)
                return NotFound();
            ViewData["StartDate"] = today;
            ViewData["FinishDate"] = tomorrow;
            ViewData["Extralar"] = _context.Extralars.ToList();
            ViewData["AlisLokasyonId"] = new SelectList(_context.Lokasyonlars.Where(x => x.Durum == 1).Select(x => new { x.Id, AdFiyat = $"{x.Ad} - {x.Fiyat} ₺" }).ToList(), "Id", "AdFiyat");
            ViewData["Kasko"]=new SelectList(_context.Kaskolars.Select(x => new { x.Id, AdFiyat = $"{x.Ad} - {x.GunlukFiyat} ₺" }).ToList(), "Id", "AdFiyat");
            ViewData["Arac"] = arac;
            ViewData["TeslimLokasyonId"] = new SelectList(_context.Lokasyonlars.Where(x => x.Durum == 1).Select(x => new { x.Id, AdFiyat = $"{x.Ad} - {x.Fiyat} ₺" }).ToList(), "Id", "AdFiyat");
            ViewData["User"] = (User.Identity.IsAuthenticated) ? User.FindFirst(ClaimTypes.Sid).Value : -1;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout([FromForm] Rezervasyon rezervasyon)
        {
            ViewData["StartDate"] = rezervasyon.StartDate?.ToString("yyyy-mm-dd");
            ViewData["FinishDate"] = rezervasyon.FinishDate?.ToString("yyyy-mm-dd");
            ViewData["AlisLokasyonId"] = new SelectList(_context.Lokasyonlars, "Id", "Ad");
            var arac = _context.Araclars.Include(r => r.Resims).FirstOrDefault(r => r.Id == rezervasyon.AracId);
            ViewData["Arac"] = arac;
            ViewData["Extralar"] = _context.Extralars.ToList();
            ViewData["TeslimLokasyonId"] = new SelectList(_context.Lokasyonlars, "Id", "Ad");
            ViewData["User"] = (User.Identity.IsAuthenticated) ? User.FindFirst(ClaimTypes.Sid).Value : -1;
            if (ModelState.IsValid)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var user = _context.Users.FirstOrDefault(x => x.Id == Convert.ToInt32(User.FindFirst(ClaimTypes.Sid).Value));
                    rezervasyon.User = user;
                }
                else
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
                    rezervasyon.UserId = rezervasyon.User.Id;
                }
                var aracget = _context.Araclars.FirstOrDefault(x=>x.Id==rezervasyon.AracId);
                if (aracget == null)
                    return NotFound("Böyle bir araç yok");
                rezervasyon.Arac= aracget;
                var alokasyon = _context.Lokasyonlars.FirstOrDefault(x => x.Id == rezervasyon.AlisLokasyonId);
                if (alokasyon == null)
                    return NotFound("Böyle bir lokasyon yok");
                rezervasyon.AlisLokasyon= alokasyon;
                var tlokasyon=_context.Lokasyonlars.FirstOrDefault(x=>x.Id==rezervasyon.TeslimLokasyonId);
                if (tlokasyon == null)
                    return NotFound("Böyle bir lokasyon yok");
                rezervasyon.TeslimLokasyon = tlokasyon;
                var gunSayisi = Math.Max(1, (rezervasyon.FinishDate - rezervasyon.StartDate)?.Days ?? 0);///fiyat hesplaması
                rezervasyon.Fiyat = gunSayisi * (rezervasyon.Arac?.Fiyat ?? 1000);
                if (rezervasyon.SelectedExtralar != null)
                {
                    foreach (int item in rezervasyon.SelectedExtralar)
                    {
                        Extralar extralar = _context.Extralars.FirstOrDefault(x=> x.Id == item);
                            if(extralar != null)
                            {
                                rezervasyon.Extralar.Add(extralar);
                                rezervasyon.Fiyat += extralar.Fiyat;
                            }
                    }
                }
                var rezervasyonview = new RezervasyonViewModel
                {
                    UserId = rezervasyon.UserId,
                    AracId = rezervasyon.AracId,
                    StartDate = rezervasyon.StartDate??DateTime.Today,
                    FinishDate = rezervasyon.FinishDate?? DateTime.Today.AddDays(1),
                    AlisLokasyonId = rezervasyon.AlisLokasyonId??0,
                    TeslimLokasyonId = rezervasyon.TeslimLokasyonId??0,
                    KaskoId = rezervasyon.KaskoId ?? 0,
                    Fiyat = rezervasyon.Fiyat??0,
                    Extralar = rezervasyon.SelectedExtralar
                };

                var rezervasyonJson = JsonSerializer.Serialize(rezervasyonview);
                Response.Cookies.Append("rezervasyon", rezervasyonJson, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTimeOffset.Now.AddMinutes(10)
                });

                var formHtml = await _paymentService.CreateCheckoutForm(rezervasyon);
            ViewBag.CheckoutForm = formHtml;
            return View();
            }
            return View(rezervasyon);
        }

        [HttpPost]
        public async Task<IActionResult> Callback()
        {
            var token = Request.Form["token"];
            if (string.IsNullOrEmpty(token))
            {
                ViewBag.Message = "Ödeme sırasında bir hata oluştu (token alınamadı).";
                return View();
            }

            var checkoutForm = await _paymentService.RetrieveCheckoutForm(token);

            if (checkoutForm.Status == "success")
            {
                if (Request.Cookies.TryGetValue("rezervasyon", out var rezervasyonJson))
                {
                    var rezervasyonview = JsonSerializer.Deserialize<RezervasyonViewModel>(rezervasyonJson);
                    var rezervasyon = new Rezervasyon
                    {
                        UserId = rezervasyonview.UserId,
                        AracId = rezervasyonview.AracId,
                        StartDate = rezervasyonview.StartDate,
                        FinishDate = rezervasyonview.FinishDate,
                        AlisLokasyonId = rezervasyonview.AlisLokasyonId,
                        TeslimLokasyonId = rezervasyonview.TeslimLokasyonId,
                        KaskoId = rezervasyonview.KaskoId,
                        Fiyat = rezervasyonview.Fiyat,
                    };
                    if (rezervasyonview.Extralar != null && rezervasyonview.Extralar.Count > 0) {
                        foreach (int item in rezervasyonview.Extralar)
                        {
                            Extralar extralar = _context.Extralars.FirstOrDefault(e=>e.Id == item);
                            if (extralar != null) { 
                                rezervasyon.Extralar.Add(extralar);
                            }
                        }
                    }
                _context.Add(rezervasyon);
                _context.SaveChanges();
                ViewBag.Message = "Ödeme başarılı! Rezervasyon eklendi";
                }
                else
                {
                ViewBag.Message = "Ödeme başarılı! Rezervasyon eklenemedi!!!!";
                }
                // Burada rezervasyon kaydı yapılabilir
            }
            else
            {
                ViewBag.Message = "Ödeme başarısız: " + checkoutForm.ErrorMessage;
            }

            return View();
        }


    }
}
