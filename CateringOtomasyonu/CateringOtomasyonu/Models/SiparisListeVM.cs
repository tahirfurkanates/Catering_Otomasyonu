using System;

namespace CateringOtomasyonu.Models
{
    public class SiparisListeVM
    {
        public int EtkinlikId { get; set; }
        public DateTime Baslangic { get; set; }
        public DateTime Bitis { get; set; }
        public string Konum { get; set; } = "-";
        public int? KisiSayisi { get; set; }
        public string Durum { get; set; } = "-";
    }
}
