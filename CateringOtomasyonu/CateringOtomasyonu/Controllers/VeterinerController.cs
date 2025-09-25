using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CateringOtomasyonu.dal;
using CateringOtomasyonu.Models;

public class VeterinerController : BaseController
{
    private readonly VeterinerOtomasyonuContext _context;

    public VeterinerController(VeterinerOtomasyonuContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        if (!IsInRole("Admin"))
            return DenyAccess();
        var veterinerler = _context.Veterinerlers.ToList();
        return View(veterinerler);
    }
    [HttpGet]
    public IActionResult Ekle()
    {
        if (!IsInRole("Admin"))
            return DenyAccess();
        return View();
    }

    [HttpPost]
    public IActionResult Ekle(VeterinerKullaniciViewModel model)
    {
        if (!IsInRole("Admin"))
            return DenyAccess();
        if (!ModelState.IsValid)
            return View(model);

        // 1. Veterineri oluştur
        var veteriner = new Veterinerler
        {
            Ad = model.Ad,
            Soyad = model.Soyad,
            Telefon = model.Telefon,
            Email = model.Email,
            Uzmanlik = model.Uzmanlik
        };

        _context.Veterinerlers.Add(veteriner);
        _context.SaveChanges(); // ⬅️ ID oluşur

        // 2. Kullanıcıyı oluştur ve veteriner ile ilişkilendir
        var user = new User
        {
            KullaniciAdi = model.KullaniciAdi,
            Sifre = model.Sifre,
            Rol = "Veteriner",
            VeterinerId = veteriner.VeterinerId
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        // 3. Aktivite kaydı ekle
        _context.Aktivitelers.Add(new Aktivite
        {
            UserId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0"),
            KullaniciAdi = HttpContext.Session.GetString("KullaniciAdi") ?? "Bilinmiyor",
            Rol = HttpContext.Session.GetString("Rol") ?? "Bilinmiyor",
            Islem = $"Yeni veteriner eklendi: {veteriner.Ad} {veteriner.Soyad}",
            Tarih = DateTime.Now
        });

        _context.SaveChanges();

        TempData["Mesaj"] = "Veteriner ve kullanıcı başarıyla eklendi.";
        return RedirectToAction("Ekle");
    }
    public IActionResult Sil()
    {
        if (!IsInRole("Admin"))
            return DenyAccess();

        var model = _context.Veterinerlers
            .Select(v => new VeterinerViewModel
            {
                Id = v.VeterinerId,
                Ad = v.Ad + " " + v.Soyad,
                Uzmanlik = v.Uzmanlik,

            })
            .ToList();

        return View(model);
    }

    [HttpPost]
    public IActionResult SilOnay(int id)
    {
        if (!IsInRole("Admin"))
            return DenyAccess();
        var veteriner = _context.Veterinerlers.FirstOrDefault(v => v.VeterinerId == id);
        if (veteriner != null)
        {
            // Önce ilişkili kullanıcıları sil
            var kullanicilar = _context.Users.Where(u => u.VeterinerId == veteriner.VeterinerId).ToList();
            _context.Users.RemoveRange(kullanicilar);

            // Sonra veterineri sil
            _context.Veterinerlers.Remove(veteriner);

            // Aktivite kaydı
            int.TryParse(HttpContext.Session.GetString("UserId"), out int userId);
            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi") ?? "Bilinmiyor";
            var rol = HttpContext.Session.GetString("Rol") ?? "Bilinmiyor";

            if (userId > 0)
            {
                _context.Aktivitelers.Add(new Aktivite
                {
                    UserId = userId,
                    KullaniciAdi = kullaniciAdi,
                    Rol = rol,
                    Islem = $"Veteriner silindi: {veteriner.Ad} {veteriner.Soyad}",
                    Tarih = DateTime.Now
                });
            }

            _context.SaveChanges();

            TempData["SilMesaji"] = $"✅ {veteriner.Ad} {veteriner.Soyad} başarıyla silindi.";
        }

        return RedirectToAction("Sil");
    }
    public IActionResult Guncelle()
    {
        if (!IsInRole("Admin"))
            return DenyAccess();
        var model = _context.Veterinerlers
            .Select(v => new VeterinerGuncelleViewModel
            {
                VeterinerId = v.VeterinerId,
                Ad = v.Ad,
                Soyad = v.Soyad,
                Telefon = v.Telefon,
                Email = v.Email,
                Uzmanlik = v.Uzmanlik
            })
            .ToList();

        return View(model);
    }

    // POST: Veteriner/Guncelle
    [HttpPost]
    public IActionResult Guncelle(VeterinerGuncelleViewModel model)
    {
        if (!IsInRole("Admin"))
            return DenyAccess();
        if (!ModelState.IsValid)
        {
            var list = _context.Veterinerlers
                .Select(v => new VeterinerGuncelleViewModel
                {
                    VeterinerId = v.VeterinerId,
                    Ad = v.Ad,
                    Soyad = v.Soyad,
                    Telefon = v.Telefon,
                    Email = v.Email,
                    Uzmanlik = v.Uzmanlik
                }).ToList();

            return View(list);
        }

        var veteriner = _context.Veterinerlers.FirstOrDefault(v => v.VeterinerId == model.VeterinerId);
        if (veteriner != null)
        {
            veteriner.Ad = model.Ad;
            veteriner.Soyad = model.Soyad;
            veteriner.Telefon = model.Telefon;
            veteriner.Email = model.Email;
            veteriner.Uzmanlik = model.Uzmanlik;

            _context.SaveChanges();

            // 🔻 Aktivite Log
            int.TryParse(HttpContext.Session.GetString("UserId"), out int userId);
            var kullaniciAdi = HttpContext.Session.GetString("KullaniciAdi") ?? "Bilinmiyor";
            var rol = HttpContext.Session.GetString("Rol") ?? "Bilinmiyor";

            if (userId > 0)
            {
                _context.Aktivitelers.Add(new Aktivite
                {
                    UserId = userId,
                    KullaniciAdi = kullaniciAdi,
                    Rol = rol,
                    Islem = $"Veteriner güncellendi: {veteriner.Ad} {veteriner.Soyad}",
                    Tarih = DateTime.Now
                });

                _context.SaveChanges();
            }

            TempData["GuncelleMesaji"] = $"✅ {veteriner.Ad} {veteriner.Soyad} başarıyla güncellendi.";
        }

        return RedirectToAction("Guncelle");
    }
}
