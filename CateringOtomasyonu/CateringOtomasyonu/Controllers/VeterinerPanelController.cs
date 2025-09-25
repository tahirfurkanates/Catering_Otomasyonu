using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CateringOtomasyonu.dal;
using CateringOtomasyonu.ViewModels;
using System;
using System.Linq;
using CateringOtomasyonu.db;

namespace CateringOtomasyonu.Controllers
{
    public class VeterinerPanelController : BaseController
    {
        private readonly CateringDbContext _context;

        public VeterinerPanelController(CateringDbContext context)
        {
            _context = context;
        }
       

        public IActionResult Index()
        {
            if (!IsInRole("Veteriner"))
                return DenyAccess();

            // Kullanıcı bilgisi
            int userId = int.Parse(HttpContext.Session.GetString("UserId") ?? "0");
            var user = _context.Personellers.FirstOrDefault(x => x.PersonelId == userId);

            if (user == null || user.PersonelId == null)
                return RedirectToAction("Giris", "Login");

            // Bugünün dolu randevularını çekiyoruz
         

            
            return View();
        }
    }
}