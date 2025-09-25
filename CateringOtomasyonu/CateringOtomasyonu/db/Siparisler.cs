using System;
using System.Collections.Generic;

namespace CateringOtomasyonu.db;

public partial class Siparisler
{
    public int SiparisId { get; set; }

    public int EtkinlikId { get; set; }

    public int OlusturanId { get; set; }

    public DateTime OlusturmaTarihi { get; set; }

    public decimal ToplamTutar { get; set; }

    public string Durum { get; set; } = null!;

    public virtual Etkinlikler Etkinlik { get; set; } = null!;

    public virtual ICollection<SiparisKalemleri> SiparisKalemleris { get; set; } = new List<SiparisKalemleri>();
}
