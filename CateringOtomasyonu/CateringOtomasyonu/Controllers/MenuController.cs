using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CateringOtomasyonu.db;
using CateringOtomasyonu.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CateringOtomasyonu.Controllers
{
    public class MenuController : Controller
    {
        private readonly CateringDbContext _db;
        private readonly IWebHostEnvironment _env;

        public MenuController(CateringDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        // *** DETAY ***
        // Attribute route YOK; Program.cs'deki
        // app.MapControllerRoute("menu-details","menu/{id:int}/{slug?}", ...) bu aksiyona yönlendiriyor.
        [HttpGet]
        public IActionResult Details(int id, string? slug)
        {
            var item = _db.MenuOgeleri.FirstOrDefault(x => x.MenuOgeleriId == id);
            if (item == null) return NotFound();

            var similar = _db.MenuOgeleri
                             .Where(x => x.MenuOgeleriId != id)
                             .OrderByDescending(x => x.MenuOgeleriId)
                             .Take(6)
                             .ToList();

            return View(new MenuDetailsVM { Item = item, Similar = similar });
        }

        // *** TOPLU GÖRSEL YÜKLE (GET) ***
        // Şablon YOK; conventional route ile /Menu/TopluResimYukle
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult TopluResimYukle()
        {
            ViewData["Title"] = "Toplu Görsel Yükle";
            return View();
        }

        // *** TOPLU GÖRSEL YÜKLE (POST) ***
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TopluResimYukle(List<IFormFile> files)
        {
            if (files == null || files.Count == 0)
            {
                TempData["Uyari"] = "Dosya seçmediniz.";
                return RedirectToAction(nameof(TopluResimYukle));
            }

            var folder = Path.Combine(_env.WebRootPath, "img", "menu");
            Directory.CreateDirectory(folder);

            var allowed = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            int ok = 0, skip = 0;

            foreach (var f in files)
            {
                if (f == null || f.Length == 0) { skip++; continue; }

                var ext = Path.GetExtension(f.FileName).ToLowerInvariant();
                if (!allowed.Contains(ext)) { skip++; continue; }

                var name = Path.GetFileNameWithoutExtension(f.FileName);
                var digits = new string(name.Where(char.IsDigit).ToArray());
                if (string.IsNullOrEmpty(digits) || !int.TryParse(digits, out var id)) { skip++; continue; }

                var item = _db.MenuOgeleri.FirstOrDefault(x => x.MenuOgeleriId == id);
                if (item == null) { skip++; continue; }

                foreach (var exist in Directory.GetFiles(folder, id + ".*"))
                    System.IO.File.Delete(exist);

                var savePath = Path.Combine(folder, $"{id}{ext}");
                using (var fs = new FileStream(savePath, FileMode.Create))
                    await f.CopyToAsync(fs);

                var prop = item.GetType().GetProperty("ResimYolu");
                if (prop != null) prop.SetValue(item, $"/img/menu/{id}{ext}");

                ok++;
            }

            _db.SaveChanges();
            TempData["Mesaj"] = $"{ok} dosya yüklendi, {skip} dosya atlandı.";
            return RedirectToAction(nameof(TopluResimYukle));
        }
    }
}
