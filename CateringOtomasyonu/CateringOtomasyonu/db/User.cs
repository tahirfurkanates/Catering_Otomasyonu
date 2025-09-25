using System;
using System.Collections.Generic;

namespace CateringOtomasyonu.db;

public partial class User
{
    public int Id { get; set; }

    public string KullaniciAdi { get; set; } = null!;

    public string Sifre { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public int? PersonelId { get; set; }

    public int? MusteriId { get; set; }

    public virtual ICollection<Aktiviteler> Aktivitelers { get; set; } = new List<Aktiviteler>();

    public virtual Musteriler? Musteri { get; set; }

    public virtual Personeller? Personel { get; set; }

    public virtual ICollection<Siparisler> Siparislers { get; set; } = new List<Siparisler>();
}
