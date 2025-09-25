using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

using CateringOtomasyonu.db;
using CateringOtomasyonu.Models;
using CateringOtomasyonu.Infrastructure;

namespace CateringOtomasyonu.Controllers
{
    public class SiparisController : Controller
    {
        private readonly CateringDbContext _db;
        private const string CART_KEY = "CART";

        public SiparisController(CateringDbContext db) => _db = db;

        // --- Sepet (önceki kurduğumuz akış) ---
        public IActionResult Ekle(int id, int adet = 1)
        {
            if (adet < 1) adet = 1;

            var item = _db.MenuOgeleri.FirstOrDefault(x => x.MenuOgeleriId == id);
            if (item == null) return NotFound();

            var cart = HttpContext.Session.GetObject<List<CartItem>>(CART_KEY) ?? new List<CartItem>();
            var existing = cart.FirstOrDefault(c => c.MenuOgeleriId == id);

            if (existing == null)
            {
                decimal fiyat = item.BirimFiyat; // non-nullable decimal
                cart.Add(new CartItem { MenuOgeleriId = id, Ad = item.Ad, Adet = adet, BirimFiyat = fiyat });
            }
            else
            {
                existing.Adet += adet;
            }

            HttpContext.Session.SetObject(CART_KEY, cart);
            HttpContext.Session.SetInt32("CartCount", cart.Sum(c => c.Adet));

            TempData["feedback-ok"] = $"{item.Ad} sepete eklendi.";
            var referer = Request.Headers["Referer"].ToString();
            if (!string.IsNullOrEmpty(referer)) return Redirect(referer);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Sepet()
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CART_KEY) ?? new List<CartItem>();
            ViewBag.Toplam = cart.Sum(x => x.Tutar);
            return View(cart);
        }

        public IActionResult Sil(int id)
        {
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CART_KEY) ?? new List<CartItem>();
            cart.RemoveAll(x => x.MenuOgeleriId == id);
            HttpContext.Session.SetObject(CART_KEY, cart);
            HttpContext.Session.SetInt32("CartCount", cart.Sum(c => c.Adet));
            return RedirectToAction(nameof(Sepet));
        }

        public IActionResult Temizle()
        {
            HttpContext.Session.Remove(CART_KEY);
            HttpContext.Session.SetInt32("CartCount", 0);
            return RedirectToAction(nameof(Sepet));
        }

        public IActionResult AdetGuncelle(int id, int adet = 1)
        {
            if (adet < 1) adet = 1;
            var cart = HttpContext.Session.GetObject<List<CartItem>>(CART_KEY) ?? new List<CartItem>();
            var item = cart.FirstOrDefault(x => x.MenuOgeleriId == id);
            if (item != null) item.Adet = adet;
            HttpContext.Session.SetObject(CART_KEY, cart);
            HttpContext.Session.SetInt32("CartCount", cart.Sum(c => c.Adet));
            return RedirectToAction(nameof(Sepet));
        }

        // --- Siparişlerim ---
        public IActionResult Siparislerim()
        {
            // Kullanıcıdan MusteriId'yi gerekirse oluşturup bağla
            int? musteriId = null;
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (int.TryParse(userIdStr, out var personelId))
            {
                var personel = _db.Personellers.FirstOrDefault(p => p.PersonelId == personelId);
                if (personel != null)
                {
                    if (!personel.MusteriId.HasValue)
                    {
                        var m = new Musteriler
                        {
                            Ad = $"{personel.Ad} {personel.Soyad}",
                            Yetkili = $"{personel.Ad} {personel.Soyad}",
                            Telefon = personel.Telefon,
                            Email = personel.Email,
                            Adres = "-",
                            VergiNo = "-"
                        };
                        _db.Musterilers.Add(m);
                        _db.SaveChanges();
                        personel.MusteriId = m.MusteriId;
                        _db.SaveChanges();
                    }
                    musteriId = personel.MusteriId;
                }
            }

            var role = HttpContext.Session.GetString("Role");
            var q = _db.Etkinliklers.AsQueryable();

            // Admin tümünü görsün; değilse sadece kendisininki
            if (!string.Equals(role, "Admin", System.StringComparison.OrdinalIgnoreCase))
            {
                q = q.Where(e => e.MusteriId == musteriId);
            }

            var list = q
                .OrderByDescending(e => e.Baslangic)
                .Select(e => new CateringOtomasyonu.Models.SiparisListeVM
                {
                    EtkinlikId = e.EtkinlikId,
                    Baslangic = e.Baslangic,
                    Bitis = e.Bitis,
                    Konum = e.Konum ?? "-",
                    KisiSayisi = e.KisiSayisi,
                    Durum = e.Durum ?? "-"
                })
                .ToList();

            return View(list);
        }
    }
}
