using HomeWork_07_13_2022.Models.Base;

namespace Pronia.Models
{
    public class BasketItem:BaseEntity
    {
        public int PlantId { get; set; }
        public Plant Plant { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
