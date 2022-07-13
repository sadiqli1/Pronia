using System.Collections.Generic;
using HomeWork_07_13_2022.Models.Base;

namespace Pronia.Models
{
    public class Category:BaseEntity
    {
        public string Name { get; set; }
        public List<PlantCategory> PlantCategories { get; set; }
    }
}
