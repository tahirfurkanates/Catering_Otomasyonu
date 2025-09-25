using System;

namespace CateringOtomasyonu.ViewModels
{
    public class FeedbackItemVM
    {
        public int Id { get; set; }
        public string? AdSoyad { get; set; }
        public string? Email { get; set; }
        public string? Mesaj { get; set; }
        public DateTime Tarih { get; set; }
    }
}
