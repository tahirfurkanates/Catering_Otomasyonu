using System;
using System.Collections.Generic;

namespace CateringOtomasyonu.db;

public partial class Personeller
{
    public int PersonelId { get; set; }

    public string Ad { get; set; } = null!;

    public string Soyad { get; set; } = null!;

    public string? Telefon { get; set; }

    public string Email { get; set; } = null!;

    public string Gorev { get; set; } = null!;

    public string? Sifre { get; set; }

    public int? MusteriId { get; set; }

    public virtual Musteriler? Musteri { get; set; }
}
