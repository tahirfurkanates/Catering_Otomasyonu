using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CateringOtomasyonu.Models
{
    [Table("OneriSikayetler")]                    // tablonun gerçek adı
    public class OneriSikayet
    {
        [Key]
        [Column("Id")]                             // ← PK sütunun gerçek adı (ör: Id / OneriSikayetID / MesajId)
        public int OneriSikayetId { get; set; }

        [Column("AdSoyad")] public string AdSoyad { get; set; } = "";
        [Column("Email")] public string? Email { get; set; }
        [Column("Mesaj")] public string Mesaj { get; set; } = "";

        [Column("OlusturmaTarihi")]               // DB'de farklıysa değiştir (örn: Tarih)
        public DateTime OlusturmaTarihi { get; set; } = DateTime.Now;

        [Column("Okundu")]                        // DB'de farklıysa değiştir (örn: OkunduMu)
        public bool Okundu { get; set; } = false;
    }
}
