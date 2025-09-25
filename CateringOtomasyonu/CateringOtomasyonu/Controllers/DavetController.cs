using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using CateringOtomasyonu.db;
using CateringOtomasyonu.ViewModels;

namespace CateringOtomasyonu.Controllers
{
    // Giriş zorunlu
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class DavetController : Controller
    {
        private readonly CateringDbContext _db;
        public DavetController(CateringDbContext db) => _db = db;

        // ---- KULLANICI OLUŞTUR (aynı senin önceki kod mantığı) ----
        [HttpGet]
        public IActionResult Olustur()
        {
            var ascilar = _db.Personellers
                .Where(p => p.Gorev == "Aşçı" || p.Gorev == "Asci" || p.Gorev == "Aşci")
                .Select(p => new SelectListItem { Value = p.PersonelId.ToString(), Text = p.Ad + " " + p.Soyad })
                .ToList();

            ViewBag.Ascilar = ascilar;
            ViewBag.BaslangicVarsayilan = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            ViewBag.BitisVarsayilan = DateTime.Now.AddHours(1).ToString("yyyy-MM-ddTHH:mm:ss");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Olustur(DateTime Baslangic, DateTime Bitis, string Konum, int[]? AsciIds)
        {
            var uid = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(uid))
                return RedirectToAction("Giris", "Login", new { ReturnUrl = Url.Action(nameof(Olustur), "Davet") });

            int userId = int.Parse(uid);
            var personel = _db.Personellers.FirstOrDefault(p => p.PersonelId == userId);

            var e = new Etkinlikler
            {
                MusteriId = personel?.MusteriId,
                Baslangic = Baslangic,
                Bitis = Bitis,
                Konum = string.IsNullOrWhiteSpace(Konum) ? null : Konum.Trim(),
                Durum = null
            };
            _db.Etkinliklers.Add(e);
            _db.SaveChanges();

            TempData["Mesaj"] = "✅ Davet oluşturuldu.";
            return RedirectToAction(nameof(Olustur));
        }

        // ---- ADMIN İŞLEMLERİ: LİSTE / DÜZENLE / SİL ----
        [Authorize(Roles= "Admin")]
        [HttpGet]
        public IActionResult Listele(string? q)
        {
            var query = from e in _db.Etkinliklers
                        join m in _db.Musterilers on e.MusteriId equals m.MusteriId into mj
                        from m in mj.DefaultIfEmpty()
                        orderby e.Baslangic descending
                        select new DavetListItemVM
                        {
                            EtkinlikId = e.EtkinlikId,
                            Baslangic = e.Baslangic,
                            Bitis = e.Bitis,
                            Konum = e.Konum,
                            Durum = e.Durum,
                            MusteriAd = m != null ? m.Ad : null
                        };
            if (!string.IsNullOrWhiteSpace(q))
            {
                var s = q.Trim().ToLower();
                query = query.Where(x =>
                    (x.Konum ?? "").ToLower().Contains(s) ||
                    (x.MusteriAd ?? "").ToLower().Contains(s));
            }

            var list = query.AsNoTracking().Take(1000).ToList(); // basit sınır
            ViewData["Title"] = "Davetleri Listele";
            ViewBag.q = q;
            return View(list);
        }

        [Authorize(Roles= "Admin")]
        [HttpGet]
        public IActionResult Duzenle(int id)
        {
            var e = _db.Etkinliklers.FirstOrDefault(x => x.EtkinlikId == id);
            if (e == null) return NotFound();

            var vm = new DavetEditVM
            {
                EtkinlikId = e.EtkinlikId,
                Baslangic = e.Baslangic,
                Bitis = e.Bitis,
                Konum = e.Konum,
                Durum = e.Durum
            };
            ViewData["Title"] = "Davet Düzenle";
            return View(vm);
        }

        [Authorize(Roles= "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Duzenle(DavetEditVM vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var e = _db.Etkinliklers.FirstOrDefault(x => x.EtkinlikId == vm.EtkinlikId);
            if (e == null) return NotFound();

            e.Baslangic = vm.Baslangic;
            e.Bitis = vm.Bitis ?? e.Bitis;
            e.Konum = string.IsNullOrWhiteSpace(vm.Konum) ? null : vm.Konum.Trim();
            e.Durum = string.IsNullOrWhiteSpace(vm.Durum) ? null : vm.Durum.Trim();
            _db.SaveChanges();

            TempData["Mesaj"] = "✅ Güncellendi.";
            return RedirectToAction(nameof(Listele));
        }

        [Authorize(Roles= "Admin")]
        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Sil(int id)
        {
            var e = _db.Etkinliklers.FirstOrDefault(x => x.EtkinlikId == id);
            if (e == null) return NotFound();

            _db.Etkinliklers.Remove(e);
            _db.SaveChanges();
            TempData["Mesaj"] = "🗑️ Silindi.";
            return RedirectToAction(nameof(Listele));
        }
    }
}
