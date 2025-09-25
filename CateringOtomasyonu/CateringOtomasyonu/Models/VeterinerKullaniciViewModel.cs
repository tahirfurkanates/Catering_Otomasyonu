using System.ComponentModel.DataAnnotations;
namespace CateringOtomasyonu.Models
{
    

    public class VeterinerKullaniciViewModel
    {
        // Veteriner bilgileri
        [Required]
        public string Ad { get; set; }

        [Required]
        public string Soyad { get; set; }

        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Uzmanlik { get; set; }

        // Giriþ bilgileri
        [Required]
        public string KullaniciAdi { get; set; }

        [Required]
        public string Sifre { get; set; }
    }

}
