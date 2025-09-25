using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

using CateringOtomasyonu.db;
using CateringOtomasyonu.ViewModels; // KayitOlViewModel kullanıyorsan

namespace CateringOtomasyonu.Controllers
{
    public class LoginController : Controller
    {
        private readonly CateringDbContext _context;
        public LoginController(CateringDbContext context) => _context = context;

        [HttpGet]
        public IActionResult Giris() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Giris(string kullaniciAdi, string sifre, string? returnUrl)
        {
            var user = _context.Personellers
                .FirstOrDefault(x => x.Email == kullaniciAdi && x.Sifre == sifre);

            if (user == null)
            {
                ViewBag.Hata = "❌ Kullanıcı adı veya şifre hatalı.";
                return View();
            }

            // Session (navbar için)
            HttpContext.Session.SetString("UserId", user.PersonelId.ToString());
            HttpContext.Session.SetString("KullaniciAdi", $"{user.Ad} {user.Soyad}");
            HttpContext.Session.SetString("Rol", user.Gorev ?? "Müşteri");
            HttpContext.Session.SetString("Role", user.Gorev ?? "Müşteri");

            // Cookie auth
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.PersonelId.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Ad} {user.Soyad}"),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Role, user.Gorev ?? "Müşteri"),
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties { IsPersistent = true });

            // >>> Eğer giriş ReturnUrl ile geldiyse oraya geri dön
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            // Aksi halde ana sayfa
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult KayitOl() => View(new KayitOlViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult KayitOl(KayitOlViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            if (_context.Personellers.Any(u => u.Email == model.KullaniciAdi))
            {
                ViewBag.Hata = "❌ Bu email zaten alınmış.";
                return View(model);
            }

            var user = new Personeller
            {
                Ad = model.Ad,
                Soyad = model.Soyad,
                Telefon = model.Telefon,
                Email = model.KullaniciAdi,
                Sifre = model.Sifre,
                Gorev = "Müşteri"
            };
            _context.Personellers.Add(user);
            _context.SaveChanges();

            // Opsiyonel: müşteri kartı oluşturma vs…

            TempData["basari"] = "✅ Kayıt başarılı, giriş yapabilirsiniz.";
            return RedirectToAction(nameof(Giris));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cikis()
        {
            HttpContext.Session.Clear();
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Giris));
        }
    }
}
