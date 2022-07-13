using System.Collections.Generic;
using HomeWork_07_13_2022.Models.Base;

namespace Pronia.Models
{
    public class Plant:BaseEntity
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Desc { get; set; }
        public string SKU { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public int PlantInformationId { get; set; }
        public PlantInformation PlantInformation { get; set; }
        public List<PlantImage> PlantImages{ get; set; }
        public List<PlantCategory> PlantCategories { get; set; }
        public List<PlantTag> PlantTags { get; set; }

    }
}
