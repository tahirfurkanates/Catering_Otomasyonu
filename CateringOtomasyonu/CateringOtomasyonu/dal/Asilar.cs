using System;
using System.Collections.Generic;

namespace CateringOtomasyonu.dal;

public partial class Asilar
{
    public int AsiId { get; set; }

    public int? HayvanId { get; set; }

    public string? AsiAdi { get; set; }

    public DateTime? YapilmaTarihi { get; set; }

    public DateTime? SonrakiTarih { get; set; }

    public virtual Hayvanlar? Hayvan { get; set; }
}
