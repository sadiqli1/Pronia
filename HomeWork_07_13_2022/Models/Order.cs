using System;
using System.Collections.Generic;
using HomeWork_07_13_2022.Models.Base;

namespace Pronia.Models
{
    public class Order:BaseEntity
    {
        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
        public List<BasketItem> basketItems { get; set; }
        public bool? Status { get; set; }
        public string Address { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
