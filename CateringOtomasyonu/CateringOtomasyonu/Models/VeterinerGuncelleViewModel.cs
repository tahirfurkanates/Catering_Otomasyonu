using System.ComponentModel.DataAnnotations;

namespace CateringOtomasyonu.Models
{
    public class VeterinerGuncelleViewModel
    {
        public int VeterinerId { get; set; }

        [Required]
        [StringLength(50)]
        public string Ad { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Soyad { get; set; } = null!;

        [StringLength(20)]
        public string? Telefon { get; set; }

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(100)]
        public string? Uzmanlik { get; set; }
    }
}