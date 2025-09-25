using CateringOtomasyonu.Models;
using System.Collections.Generic;
using System.Diagnostics;
using CateringOtomasyonu.dal;

namespace CateringOtomasyonu.ViewModels
{
    public class RandevuAlViewModel
    {
        public int HayvanId { get; set; }
        public int UzmanlikId { get; set; }
        public int VeterinerId { get; set; }
        public DateTime Tarih { get; set; }
        public string Saat { get; set; }
        public string Notlar { get; set; }
    }

}
