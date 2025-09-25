namespace CateringOtomasyonu.Models
{
    public class CartItem
    {
        public int MenuOgeleriId { get; set; }
        public string Ad { get; set; } = "";
        public int Adet { get; set; }
        public decimal BirimFiyat { get; set; }
        public decimal Tutar => BirimFiyat * Adet;
    }
}