using HomeWork_07_13_2022.Models.Base;

namespace Pronia.Models
{
    public class PlantImage:BaseEntity
    {
        public string Name { get; set; }
        public string Alternative { get; set; }
        public bool? isMain { get; set; }
        public int PlantId { get; set; }
        public Plant Plant { get; set; }
    }
}
