using System;
using System.Collections.Generic;

namespace CateringOtomasyonu.dal;

public partial class Tedaviler
{
    public int TedaviId { get; set; }

    public int? HayvanId { get; set; }

    public int? VeterinerId { get; set; }

    public DateTime? Tarih { get; set; }

    public string? Aciklama { get; set; }

    public virtual Hayvanlar? Hayvan { get; set; }

    public virtual Veterinerler? Veteriner { get; set; }
}
