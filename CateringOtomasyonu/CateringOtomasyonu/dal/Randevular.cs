using System;
using System.Collections.Generic;

namespace CateringOtomasyonu.dal;

public partial class Randevular
{
    public int RandevuId { get; set; }

    public int? HayvanId { get; set; }

    public int? VeterinerId { get; set; }

    public DateTime? Tarih { get; set; }

    public string? Notlar { get; set; }

    public bool DoluMu { get; set; }   // ✅ doğru
     
    public virtual Hayvanlar? Hayvan { get; set; }

    public virtual Veterinerler? Veteriner { get; set; }
}
