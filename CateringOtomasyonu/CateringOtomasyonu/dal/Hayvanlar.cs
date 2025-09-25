using System;
using System.Collections.Generic;

namespace CateringOtomasyonu.dal;

public partial class Hayvanlar
{
    public int HayvanId { get; set; }

    public string? Ad { get; set; }

    public string? Tur { get; set; }

    public string? Irk { get; set; }

    public string? Cinsiyet { get; set; }

    public DateTime? DogumTarihi { get; set; }

    public int? SahipId { get; set; }

    public virtual ICollection<Asilar> Asilars { get; set; } = new List<Asilar>();

    public virtual ICollection<Randevular> Randevulars { get; set; } = new List<Randevular>();

    public virtual HayvanSahipleri? Sahip { get; set; }

    public virtual ICollection<Tedaviler> Tedavilers { get; set; } = new List<Tedaviler>();
}
