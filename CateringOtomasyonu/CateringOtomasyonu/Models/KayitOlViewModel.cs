using CateringOtomasyonu.Models;
using System.Collections.Generic;
using System.Diagnostics;
using CateringOtomasyonu.dal;

namespace CateringOtomasyonu.ViewModels
{
    public class KayitOlViewModel
    {
        // Sahip bilgileri
        public string Ad { get; set; } // Ad
        public string Soyad { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public string Adres { get; set; }

        // Kullanýcý bilgileri
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
    }


}
