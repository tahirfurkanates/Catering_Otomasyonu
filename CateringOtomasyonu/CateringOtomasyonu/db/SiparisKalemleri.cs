using System;
using System.Collections.Generic;

namespace CateringOtomasyonu.db;

public partial class SiparisKalemleri
{
    public int SiparisKalemiId { get; set; }

    public int SiparisId { get; set; }

    public int MenuOgeleriId { get; set; }

    public decimal Adet { get; set; }

    public decimal BirimFiyat { get; set; }

    public decimal? Tutar { get; set; }

    public virtual MenuOgeleri MenuOgeleri { get; set; } = null!;

    public virtual Siparisler Siparis { get; set; } = null!;
}
