using CateringOtomasyonu.Models;
using System.Collections.Generic;
using System.Diagnostics;
using CateringOtomasyonu.dal;

namespace CateringOtomasyonu.ViewModels
{
    public class MuayeneViewModel
    {
        public int? HayvanId { get; set; }
        public string HayvanAdi { get; set; }
        public string Tur { get; set; }
        public string Irk { get; set; }
        public DateTime? Tarih { get; set; }
        public string Notlar { get; set; }
    }
}




