using System;
using System.Collections.Generic;

namespace CateringOtomasyonu.dal;

public partial class Sahipler
{
    public int SahipId { get; set; }

    public string? Ad { get; set; }

    public string? Soyad { get; set; }

    public string? Telefon { get; set; }

    public string? Email { get; set; }

    public string? Adres { get; set; }

    public virtual ICollection<Hayvanlar> Hayvanlars { get; set; } = new List<Hayvanlar>();
}
