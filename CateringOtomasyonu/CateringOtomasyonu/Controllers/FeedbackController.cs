using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CateringOtomasyonu.db;            // DbContext'in namespace'i
using CateringOtomasyonu.Models;

namespace CateringOtomasyonu.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly CateringDbContext _db;
        public FeedbackController(CateringDbContext db) => _db = db;

        // ANONİM GÖNDERİM
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Gonder(string adSoyad, string? email, string mesaj)
        {
            if (string.IsNullOrWhiteSpace(adSoyad) || string.IsNullOrWhiteSpace(mesaj))
            {
                TempData["FeedbackErr"] = "Lütfen ad soyad ve mesaj giriniz.";
                return Redirect(Url.Action("Index", "Home") + "#feedback");
            }

            var rec = new OneriSikayet
            {
                AdSoyad = adSoyad.Trim(),
                Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim(),
                Mesaj = mesaj.Trim()
            };

            _db.OneriSikayetler.Add(rec);
            _db.SaveChanges();

            TempData["FeedbackOk"] = "Teşekkürler! Mesajınız bize ulaştı.";
            return Redirect(Url.Action("Index", "Home") + "#feedback");
        }

        // ADMIN LİSTE
        [Authorize(Roles = "Admin")]
        public IActionResult Listele()
        {
            var list = _db.OneriSikayetler
                          .OrderByDescending(x => x.OlusturmaTarihi)
                          .ToList();
            return View(list);
        }

        // ADMIN OKUNDU
        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Okundu(int id, bool ok = true)
        {
            var rec = _db.OneriSikayetler.Find(id);
            if (rec != null) { rec.Okundu = ok; _db.SaveChanges(); }
            return RedirectToAction(nameof(Listele));
        }

        // ADMIN SİL
        [Authorize(Roles = "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Sil(int id)
        {
            var rec = _db.OneriSikayetler.Find(id);
            if (rec != null) { _db.OneriSikayetler.Remove(rec); _db.SaveChanges(); }
            return RedirectToAction(nameof(Listele));
        }
    }
}
