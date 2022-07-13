using HomeWork_07_13_2022.Models.Base;

namespace Pronia.Models
{
    public class PlantCategory:BaseEntity
    {
        public int PlantId { get; set; }
        public Plant Plant { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
