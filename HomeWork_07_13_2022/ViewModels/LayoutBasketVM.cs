using System.Collections.Generic;
using Pronia.Models;

namespace Pronia.ViewModels
{
    public class LayoutBasketVM
    {
        public List<BasketItemVM> BasketItemVMs { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
