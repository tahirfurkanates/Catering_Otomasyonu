using System;
using System.Collections.Generic;

namespace CateringOtomasyonu.db;

public partial class IletisimMesajlari
{
    public int IletisimId { get; set; }

    public string AdSoyad { get; set; } = null!;

    public string Mesaj { get; set; } = null!;

    public DateTime Tarih { get; set; }
}
