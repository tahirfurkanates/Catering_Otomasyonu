using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CateringOtomasyonu.dal;
using CateringOtomasyonu.ViewModels;

public class HastaPanelController : BaseController
{
    private readonly VeterinerOtomasyonuContext _context;

    public HastaPanelController(VeterinerOtomasyonuContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        if (!IsInRole("Hasta"))
            return DenyAccess();

        int userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null || user.SahipId == null)
            return RedirectToAction("Giris", "Login");

        int sahipId = user.SahipId.Value;

        var hayvanIds = _context.Hayvanlars
            .Where(h => h.SahipId == sahipId)
            .Select(h => h.HayvanId)
            .ToList();

        var model = new HastaPanelViewModel
        {
            UpcomingAppointments = _context.Randevulars.Count(r =>
                r.DoluMu == true &&
                r.Tarih >= DateTime.Today &&
                r.HayvanId != null &&
                hayvanIds.Contains(r.HayvanId.Value)),

            PastExaminations = _context.Tedavilers.Count(t =>
     t.HayvanId.HasValue && hayvanIds.Contains(t.HayvanId.Value))

        };

        return View(model);
    }
    public IActionResult Randevularim()
    {
        if (!IsInRole("Hasta"))
            return DenyAccess();

        int userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);
        if (user == null || user.SahipId == null)
            return RedirectToAction("Giris", "Login");

        var sahipId = user.SahipId.Value;

        // Bu hastaya ait hayvanları çek
        var hayvanlar = _context.Hayvanlars
            .Where(h => h.SahipId == sahipId)
            .ToList();

        var hayvanIds = hayvanlar.Select(h => h.HayvanId).ToList();

        // Tüm dolu randevuları al
        var bugun = DateTime.Today;
        var randevular = _context.Randevulars
            .Where(r => r.HayvanId.HasValue && hayvanIds.Contains(r.HayvanId.Value) && r.DoluMu)
            .Include(r => r.Hayvan)
            .ToList();

        var aktif = randevular.Where(r => r.Tarih >= bugun).ToList();
        var gecmis = randevular.Where(r => r.Tarih < bugun).ToList();

        // İlgili aşılar ve tedaviler
        var asiList = _context.Asilars
            .Where(a => a.HayvanId.HasValue && hayvanIds.Contains(a.HayvanId.Value))
            .ToList();

        var tedaviList = _context.Tedavilers
            .Where(t => t.HayvanId.HasValue && hayvanIds.Contains(t.HayvanId.Value))
            .ToList();

        ViewBag.AktifRandevular = aktif;
        ViewBag.GecmisRandevular = gecmis;
        ViewBag.Asilar = asiList;
        ViewBag.Tedaviler = tedaviList;

        return View();
    }



    public IActionResult HayvanListele()
    {
        if (!IsInRole("Hasta"))
            return DenyAccess();

        int userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null || user.SahipId == null)
            return RedirectToAction("Giris", "Login");

        var hayvanlar = _context.Hayvanlars
            .Where(h => h.SahipId == user.SahipId)
            .ToList();

        return View(hayvanlar);
    }

    [HttpGet]
    public IActionResult HayvanSil()
    {
        if (!IsInRole("Hasta"))
            return DenyAccess();

        int userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (user?.SahipId == null)
            return RedirectToAction("Giris", "Login");

        var hayvanlar = _context.Hayvanlars
            .Where(h => h.SahipId == user.SahipId)
            .ToList();

        return View(hayvanlar);
    }
    [HttpPost]
    public IActionResult HayvanSil(int id)
    {
        if (!IsInRole("Hasta"))
            return DenyAccess();
        var hayvan = _context.Hayvanlars.FirstOrDefault(h => h.HayvanId == id);
        if (hayvan != null)
        {
            string ad = hayvan.Ad;
            _context.Hayvanlars.Remove(hayvan);
            _context.SaveChanges();
            TempData["Mesaj"] = $"✅ \"{ad}\" adlı hayvan silindi.";
        }

        return RedirectToAction("HayvanSil");
    }

    [HttpGet]
    public IActionResult HayvanEkle()
    {
        if (!IsInRole("Hasta"))
            return DenyAccess();

        return View();
    }
    [HttpPost]
    public IActionResult HayvanEkle(HayvanEkleViewModel model)
    {
        if (!IsInRole("Hasta"))
            return DenyAccess();

        int userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null || user.SahipId == null)
            return RedirectToAction("Giris", "Login");

        var yeniHayvan = new Hayvanlar
        {
            Ad = model.Ad,
            Tur = model.Tur,
            Irk = model.Irk,
            Cinsiyet = model.Cinsiyet,
            DogumTarihi = model.DogumTarihi,
            SahipId = user.SahipId.Value
        };

        _context.Hayvanlars.Add(yeniHayvan);
        _context.SaveChanges();

        ViewBag.Mesaj = "✅ Hayvan başarıyla eklendi.";
        return View(); // Aynı sayfada kal
    }
    [HttpGet]
    public JsonResult GetByUzmanlik(string alan)
    {
        

        var doktorlar = _context.Veterinerlers
            .Where(v => v.Uzmanlik == alan)
            .Select(v => new {
                v.VeterinerId,
                AdSoyad = v.Ad + " " + v.Soyad
            })
            .ToList();

        return Json(doktorlar);
    }

    [HttpGet]
    public JsonResult GetSaatler(int veterinerId, string tarih)
    {
        DateTime seciliTarih = DateTime.Parse(tarih);

        var tumSaatler = new List<string> {
        "09:00", "10:00", "11:00", "13:00", "14:00", "15:00", "16:00"
    };

        var doluSaatler = _context.Randevulars
            .Where(r => r.VeterinerId == veterinerId && r.Tarih.Value.Date == seciliTarih && r.DoluMu)
            .Select(r => r.Tarih.Value.ToString("HH:mm"))
            .ToList();

        var bosSaatler = tumSaatler.Except(doluSaatler).ToList();

        return Json(bosSaatler);
    }

    [HttpGet]
    public JsonResult GetTarihler(int veterinerId)
    {
        var tarihler = _context.Randevulars
            .Where(r => r.VeterinerId == veterinerId)
            .Select(r => r.Tarih.Value.Date)
            .Distinct()
            .OrderBy(t => t)
            .ToList();

        return Json(tarihler.Select(t => t.ToString("yyyy-MM-dd")));
    }
    [HttpGet]
    public JsonResult GetDoktorlar(string uzmanlik)
    {
        var doktorlar = _context.Veterinerlers
            .Where(v => v.Uzmanlik == uzmanlik)
            .Select(v => new
            {
                v.VeterinerId,
                AdSoyad = v.Ad + " " + v.Soyad
            })
            .ToList();

        return Json(doktorlar);
    }

    [HttpGet]
    public JsonResult GetBosSaatler(int veterinerId, string tarih)
    {
        DateTime seciliTarih;
        if (!DateTime.TryParse(tarih, out seciliTarih))
            return Json(new List<string>());

        var tumSaatler = new List<string> { "09:00", "10:00", "11:00", "13:00", "14:00", "15:00", "16:00" };

        var doluSaatler = _context.Randevulars
            .Where(r => r.VeterinerId == veterinerId && r.Tarih.HasValue &&
                        r.Tarih.Value.Date == seciliTarih.Date && r.DoluMu)
            .Select(r => r.Tarih.Value.ToString("HH:mm"))
            .ToList();

        var bosSaatler = tumSaatler.Except(doluSaatler).ToList();

        return Json(bosSaatler);
    }



    public IActionResult RandevuAl()
    {
        if (!IsInRole("Hasta"))
            return DenyAccess();

        int userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
        var user = _context.Users.FirstOrDefault(u => u.Id == userId);

        if (user?.SahipId == null)
            return RedirectToAction("Giris", "Login");

        var hayvanlar = _context.Hayvanlars
            .Where(h => h.SahipId == user.SahipId)
            .ToList();

        var uzmanliklar = _context.Veterinerlers
            .Select(v => v.Uzmanlik)
            .Distinct()
            .ToList();

        ViewBag.Hayvanlar = hayvanlar;
        ViewBag.Uzmanliklar = uzmanliklar;
        return View();
    }
    [HttpPost]
    public IActionResult RandevuAl(RandevuAlViewModel model)
    {
        if (!IsInRole("Hasta"))
            return DenyAccess();

        TimeSpan saat;
        if (!TimeSpan.TryParse(model.Saat, out saat))
            return BadRequest("Saat formatı geçersiz.");

        var randevu = _context.Randevulars.FirstOrDefault(r =>
            r.VeterinerId == model.VeterinerId &&
            r.Tarih.HasValue &&
            r.Tarih.Value.Date == model.Tarih.Date &&
            r.Tarih.Value.TimeOfDay == saat &&
            r.DoluMu == false
        );

        if (randevu == null)
            return NotFound("Bu randevu mevcut değil veya dolu.");

        // Güncelleme
        randevu.DoluMu = true;
        randevu.HayvanId = model.HayvanId;
        randevu.Notlar = model.Notlar;

        _context.SaveChanges();

        TempData["basari"] = "✅ Randevunuz başarıyla alındı.";
        return RedirectToAction("Randevularim");
    }

    [HttpGet]
    public IActionResult Profil()
    {
        if (!IsInRole("Hasta"))
            return DenyAccess();
        int userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
        var user = _context.Users.Include(u => u.Sahip).FirstOrDefault(u => u.Id == userId);

        if (user == null || user.Sahip == null)
            return RedirectToAction("Giris", "Login");

        var model = new HastaProfilViewModel
        {
            Adres = user.Sahip.Adres,
            Email = user.Sahip.Email,
            Telefon = user.Sahip.Telefon
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Profil(HastaProfilViewModel model)
    {
        if (!IsInRole("Hasta"))
            return DenyAccess();
        int userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
        var user = _context.Users.Include(u => u.Sahip).FirstOrDefault(u => u.Id == userId);

        if (user == null || user.Sahip == null)
            return RedirectToAction("Giris", "Login");

        // Şifre değişikliği kontrolü
        if (!string.IsNullOrEmpty(model.EskiSifre))
        {
            if (user.Sifre != model.EskiSifre)
            {
                ModelState.AddModelError("", "❌ Eski şifreniz yanlış.");
                return View(model);
            }

            if (model.YeniSifre != model.YeniSifreTekrar)
            {
                ModelState.AddModelError("", "❌ Yeni şifreler eşleşmiyor.");
                return View(model);
            }

            user.Sifre = model.YeniSifre;
        }

        user.Sahip.Adres = model.Adres;
        user.Sahip.Email = model.Email;
        user.Sahip.Telefon = model.Telefon;

        _context.SaveChanges();
        TempData["basari"] = "✅ Bilgileriniz güncellendi.";
        return RedirectToAction("Profil");
    }




}
