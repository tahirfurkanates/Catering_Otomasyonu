using Microsoft.AspNetCore.Mvc;
using CateringOtomasyonu.dal;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Linq;

namespace CateringOtomasyonu.Controllers
{
    public class RandevuController : BaseController
    {
        private readonly VeterinerOtomasyonuContext _context;

        public RandevuController(VeterinerOtomasyonuContext context)
        {
            _context = context;
        }

        public IActionResult Olustur()
        {
            if (!(IsInRole("Admin") || !IsInRole("Doktor")))
                return DenyAccess();

            if (IsInRole("Admin"))
                ViewBag.Veterinerler = _context.Veterinerlers.ToList();

            return View();
        }
    }
}
