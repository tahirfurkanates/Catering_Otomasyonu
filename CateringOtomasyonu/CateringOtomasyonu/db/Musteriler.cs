using System;
using System.Collections.Generic;

namespace CateringOtomasyonu.db;

public partial class Musteriler
{
    public int MusteriId { get; set; }

    public string Ad { get; set; } = null!;

    public string? Yetkili { get; set; }

    public string? Telefon { get; set; }

    public string? Email { get; set; }

    public string? Adres { get; set; }

    public string? VergiNo { get; set; }

    public virtual ICollection<Etkinlikler> Etkinliklers { get; set; } = new List<Etkinlikler>();

    public virtual ICollection<Personeller> Personellers { get; set; } = new List<Personeller>();
}
