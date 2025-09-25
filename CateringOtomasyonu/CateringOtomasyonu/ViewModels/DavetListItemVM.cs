using System;

namespace CateringOtomasyonu.ViewModels
{
    public class DavetListItemVM
    {
        public int EtkinlikId { get; set; }
        public DateTime Baslangic { get; set; }
        public DateTime? Bitis { get; set; }
        public string? Konum { get; set; }
        public string? Durum { get; set; }
        public string? MusteriAd { get; set; }
    }
}
