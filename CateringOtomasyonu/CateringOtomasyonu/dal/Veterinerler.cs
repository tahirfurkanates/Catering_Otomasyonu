using System;
using System.Collections.Generic;

namespace CateringOtomasyonu.dal;

public partial class Veterinerler
{
    public int VeterinerId { get; set; }

    public string? Ad { get; set; }

    public string? Soyad { get; set; }

    public string? Telefon { get; set; }

    public string? Email { get; set; }

    public string? Uzmanlik { get; set; }

    public virtual ICollection<Randevular> Randevulars { get; set; } = new List<Randevular>();

    public virtual ICollection<Tedaviler> Tedavilers { get; set; } = new List<Tedaviler>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
