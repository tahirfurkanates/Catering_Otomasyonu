using System;
using System.ComponentModel.DataAnnotations;

namespace CateringOtomasyonu.ViewModels
{
    public class DavetEditVM
    {
        public int EtkinlikId { get; set; }

        [Required]
        public DateTime Baslangic { get; set; }

        public DateTime? Bitis { get; set; }

        [MaxLength(500)]
        public string? Konum { get; set; }

        [MaxLength(100)]
        public string? Durum { get; set; }
    }
}
