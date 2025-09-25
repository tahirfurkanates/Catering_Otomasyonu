using System;
using System.Collections.Generic;

namespace CateringOtomasyonu.db;

public partial class Etkinlikler
{
    public int EtkinlikId { get; set; }

    public int? MusteriId { get; set; }

    public DateTime Baslangic { get; set; }

    public DateTime Bitis { get; set; }

    public string? Konum { get; set; }

    public int? KisiSayisi { get; set; }

    public string Durum { get; set; } = null!;

    public string? Notlar { get; set; }

    public virtual Musteriler? Musteri { get; set; }

    public virtual ICollection<Siparisler> Siparislers { get; set; } = new List<Siparisler>();
}
