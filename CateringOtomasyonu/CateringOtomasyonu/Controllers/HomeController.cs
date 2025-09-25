using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

// DbContext + entity'ler
using CateringOtomasyonu.db;
// HomePageVM
using CateringOtomasyonu.Models;

namespace CateringOtomasyonu.Controllers
{
    public class HomeController : Controller
    {
        private readonly CateringDbContext _db;
        public HomeController(CateringDbContext db) => _db = db;

        // GET: /
        public IActionResult Index()
        {
            var vm = new HomePageVM
            {
                // <<< DÜZELTİLDİ: SatinAlinmaSayisi yerine MenuOgeleriId ile sırala
                PopulerMenuOgeleri = _db.MenuOgeleri
                    .OrderByDescending(x => x.MenuOgeleriId)
                    .Take(6)
                    .ToList()
            };
            return View(vm);
        }

        // POST: /Home/Feedback
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Feedback(HomePageVM vm)
        {
            if (!ModelState.IsValid)
            {
                // <<< DÜZELTİLDİ: SatinAlinmaSayisi yerine MenuOgeleriId
                vm.PopulerMenuOgeleri = _db.MenuOgeleri
                    .OrderByDescending(x => x.MenuOgeleriId)
                    .Take(6)
                    .ToList();

                TempData["feedback-error"] = "Lütfen formu kontrol edin.";
                return View("Index", vm);
            }

            _db.IletisimMesajlari.Add(new IletisimMesajlari
            {
                AdSoyad = vm.AdSoyad,
                Mesaj = vm.Mesaj,
                Tarih = DateTime.Now
            });
            _db.SaveChanges();

            TempData["feedback-ok"] = "Teşekkürler! Mesajın alındı.";
            return RedirectToAction(nameof(Index));
        }
    }
}
