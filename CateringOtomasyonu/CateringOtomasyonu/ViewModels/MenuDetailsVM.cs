using System.Collections.Generic;
using CateringOtomasyonu.db;

namespace CateringOtomasyonu.ViewModels
{
    public class MenuDetailsVM
    {
        public MenuOgeleri Item { get; set; } = null!;
        public List<MenuOgeleri> Similar { get; set; } = new();
    }
}
