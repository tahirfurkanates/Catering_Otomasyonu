using System;
using System.Collections.Generic;

namespace CateringOtomasyonu.db;

public partial class Aktiviteler
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string KullaniciAdi { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public string Islem { get; set; } = null!;

    public DateTime Tarih { get; set; }

    public virtual User User { get; set; } = null!;
}
