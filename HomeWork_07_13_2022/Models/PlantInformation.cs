using System.Collections.Generic;
using HomeWork_07_13_2022.Models.Base;

namespace Pronia.Models
{
    public class PlantInformation:BaseEntity
    {
        public string Shipping { get; set; }
        public string AboutReturnRequest { get; set; }
        public string Guarantee { get; set; }
        public List<Plant> Plants { get; set; }
    }
}
