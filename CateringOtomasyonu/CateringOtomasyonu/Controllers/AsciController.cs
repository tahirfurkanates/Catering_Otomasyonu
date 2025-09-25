using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CateringOtomasyonu.db;

namespace CateringOtomasyonu.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AsciController : Controller
    {
        private readonly CateringDbContext _db;
        public AsciController(CateringDbContext db) => _db = db;

        public IActionResult Index()
        {
            var list = _db.Personellers
                          .Where(x => x.Gorev == "Aşçı")
                          .OrderBy(x => x.Ad)
                          .ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult Ekle() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Ekle(Personeller p)
        {
            if (!ModelState.IsValid) return View(p);

            if (_db.Personellers.Any(x => x.Email == p.Email))
            {
                ModelState.AddModelError("Email", "Bu e-posta zaten kayıtlı.");
                return View(p);
            }

            p.Gorev = "Aşçı";
            _db.Personellers.Add(p);
            _db.SaveChanges();
            TempData["Mesaj"] = "Aşçı eklendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Duzenle(int id)
        {
            var p = _db.Personellers.FirstOrDefault(x => x.PersonelId == id && x.Gorev == "Aşçı");
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Duzenle(Personeller m)
        {
            var p = _db.Personellers.FirstOrDefault(x => x.PersonelId == m.PersonelId && x.Gorev == "Aşçı");
            if (p == null) return NotFound();

            if (_db.Personellers.Any(x => x.Email == m.Email && x.PersonelId != m.PersonelId))
                ModelState.AddModelError("Email", "Bu e-posta başka bir personele ait.");

            if (!ModelState.IsValid) return View(m);

            p.Ad = m.Ad;
            p.Soyad = m.Soyad;
            p.Telefon = m.Telefon;
            p.Email = m.Email;
            p.Sifre = m.Sifre;      // not: basitleştirilmiş örnek
            _db.SaveChanges();

            TempData["Mesaj"] = "Güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Sil(int id)
        {
            var p = _db.Personellers.FirstOrDefault(x => x.PersonelId == id && x.Gorev == "Aşçı");
            if (p != null)
            {
                _db.Personellers.Remove(p);
                _db.SaveChanges();
                TempData["Mesaj"] = "Silindi.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
