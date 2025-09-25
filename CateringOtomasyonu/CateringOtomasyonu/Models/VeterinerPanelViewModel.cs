namespace CateringOtomasyonu.ViewModels
{
    public class VeterinerPanelViewModel
    {
        public int BugunkuRandevuSayisi { get; set; }
        public List<RandevuViewModel> Randevular { get; set; } = new();
    }

}