using HomeWork_07_13_2022.Models.Base;

namespace Pronia.Models
{
    public class PlantTag:BaseEntity
    {
        public int PlantId { get; set; }
        public Plant Plant { get; set; }
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
