using HomeWork_07_13_2022.Models.Base;

namespace HomeWork_07_13_2022.Models
{
    public class Slider:BaseEntity
    {
        public string Image { get; set; }
        public string Discount { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string ButtonUrl { get; set; }
        public byte Order { get; set; }
    }
}