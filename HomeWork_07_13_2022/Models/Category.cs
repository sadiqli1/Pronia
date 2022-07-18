using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HomeWork_07_13_2022.Models.Base;

namespace Pronia.Models
{
    public class Category:BaseEntity
    {
        [Required, StringLength(maximumLength:20)]
        public string Name { get; set; }
        public List<PlantCategory> PlantCategories { get; set; }
    }
}
