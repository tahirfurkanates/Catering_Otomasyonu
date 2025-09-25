using System.ComponentModel.DataAnnotations.Schema;
using CateringOtomasyonu.dal;

public class Aktivite
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string KullaniciAdi { get; set; } = null!;
    public string Rol { get; set; } = null!;
    public string Islem { get; set; } = null!;
    public DateTime Tarih { get; set; }

    [ForeignKey("UserId")]
    public virtual User Kullanici { get; set; } = null!;
}
