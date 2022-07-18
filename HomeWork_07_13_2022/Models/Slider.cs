using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HomeWork_07_13_2022.Models.Base;
using Microsoft.AspNetCore.Http;

namespace HomeWork_07_13_2022.Models
{
    public class Slider:BaseEntity
    {
        public string Image { get; set; }
        [Required(ErrorMessage = "Zehmet olmasa endirim qiymetini daxil edin")]
        public string Discount { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Desc { get; set; }
        [Required]
        public string ButtonUrl { get; set; }
        [Required]
        public byte Order { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}