using CateringOtomasyonu.db;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CateringOtomasyonu.Models
{
    public class HomePageVM
    {
        public IEnumerable<MenuOgeleri> PopulerMenuOgeleri { get; set; } = new List<MenuOgeleri>();

        [Required, StringLength(80)]
        public string AdSoyad { get; set; } = string.Empty;

        [Required, StringLength(1000)]
        public string Mesaj { get; set; } = string.Empty;
    }
}
