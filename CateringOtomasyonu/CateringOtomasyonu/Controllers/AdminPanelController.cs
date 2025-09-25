using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CateringOtomasyonu.db;

namespace CateringOtomasyonu.Controllers
{
    [Authorize(Roles= "Admin")]
    public class AdminPanelController : Controller
    {
        private readonly CateringDbContext _db;
        public AdminPanelController(CateringDbContext db) => _db = db;

        public IActionResult Index()
        {
            ViewData["Title"] = "Gösterge Paneli";
            var toplamMenu = _db.MenuOgeleri.Count();
            var toplamDavet = _db.Etkinliklers.Count();
            var bugunDavet = _db.Etkinliklers.Count(e => e.Baslangic.Date == DateTime.Today);
            ViewBag.ToplamMenu = toplamMenu;
            ViewBag.ToplamDavet = toplamDavet;
            ViewBag.BugunDavet = bugunDavet;
            return View();
        }
    }
}
