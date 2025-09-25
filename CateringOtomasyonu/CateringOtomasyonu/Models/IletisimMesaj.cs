using CateringOtomasyonu.Models;
using System.Collections.Generic;
using System.Diagnostics;
using CateringOtomasyonu.dal;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace CateringOtomasyonu.ViewModels
{

    [Table("IletisimMesajlari")] // 🔥 Tablo adı SQL'deki ile birebir
    public class IletisimMesaj
    {
        [Key]
        public int IletisimId { get; set; }

        [Required]
        public string AdSoyad { get; set; }

        [Required]
        public string Mesaj { get; set; }

        public DateTime Tarih { get; set; } = DateTime.Now;
    }



}
