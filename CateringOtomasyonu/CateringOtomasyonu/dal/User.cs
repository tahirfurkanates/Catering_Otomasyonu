using System;
using System.Collections.Generic;

namespace CateringOtomasyonu.dal;

public partial class User
{
    public int Id { get; set; }

    public string KullaniciAdi { get; set; } = null!;

    public string Sifre { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public int? VeterinerId { get; set; }

    public int? SahipId { get; set; }

    public virtual ICollection<Aktivite> Aktiviteler { get; set; } = new List<Aktivite>();

    public virtual HayvanSahipleri? Sahip { get; set; }

    public virtual Veterinerler? Veteriner { get; set; }
}
