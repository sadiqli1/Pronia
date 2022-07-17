using HomeWork_07_13_2022.Models.Base;

namespace Pronia.Models
{
    public class Settings:BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
