using System;
using System.Collections.Generic;

namespace CateringOtomasyonu.db;

public partial class MenuOgeleri
{
    public int MenuOgeleriId { get; set; }

    public string Ad { get; set; } = null!;

    public string? Kategori { get; set; }

    public decimal BirimFiyat { get; set; }

    public string? Aciklama { get; set; }

    public bool AktifMi { get; set; }

    public virtual ICollection<SiparisKalemleri> SiparisKalemleris { get; set; } = new List<SiparisKalemleri>();
}
