using CateringOtomasyonu.Models;
using System.Collections.Generic;
using System.Diagnostics;
using CateringOtomasyonu.dal;

namespace CateringOtomasyonu.ViewModels
{
    public class HayvanEkleViewModel
    {
        public string Ad { get; set; } = null!;
        public string Tur { get; set; } = null!;
        public string Irk { get; set; } = null!;
        public string Cinsiyet { get; set; } = null!;
        public DateTime? DogumTarihi { get; set; }
    }

}
