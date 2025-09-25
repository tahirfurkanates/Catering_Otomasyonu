using CateringOtomasyonu.Models;
using System.Collections.Generic;
using System.Diagnostics;
using CateringOtomasyonu.dal;

namespace CateringOtomasyonu.ViewModels
{
    public class HastaProfilViewModel
    {
        public string Adres { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }

        // Þifre deðiþimi
        public string EskiSifre { get; set; }
        public string YeniSifre { get; set; }
        public string YeniSifreTekrar { get; set; }
    }

}
