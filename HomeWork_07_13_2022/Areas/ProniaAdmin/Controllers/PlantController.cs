using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWork_07_13_2022.DAL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Extensions;
using Pronia.Models;

namespace Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class PlantController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PlantController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            List<Plant> model = await _context.Plants.Include(p => p.PlantCategories).ThenInclude(c => c.Category)
                .Include(p => p.PlantImages)
                .Include(P => P.PlantInformation)
                .Include(P => P.PlantTags).ToListAsync();
            return View(model);
        }
        public IActionResult Create()
        {
            ViewBag.Information = _context.PlantInformations.ToList();
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Plant plant)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Information = _context.PlantInformations.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                return View();
            }
            if (plant.HoverPhoto == null || plant.MainPhoto == null || plant.Photos == null)
            {
                ViewBag.Information = _context.PlantInformations.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                ModelState.AddModelError(string.Empty, "You must choose 1 main image and 1 hover image and 1 another image"); 
                return View();
            }
            if (!plant.MainPhoto.ImageisOkay(2) || !plant.HoverPhoto.ImageisOkay(2))
            {
                ViewBag.Information = _context.PlantInformations.ToList();
                ViewBag.Categories = _context.Categories.ToList();
                ModelState.AddModelError(string.Empty, "Please choose valid Image");
                return View();
            }
            foreach (IFormFile photo in plant.Photos)
            {
                if (!photo.ImageisOkay(2))
                {
                    plant.Photos.Remove(photo);
                    TempData["FileName"] = photo.FileName;
                }
            }
            PlantImage main = new PlantImage
            {
                Name = await plant.MainPhoto.FileCreate(_env.WebRootPath, "assets/images/website-images"),
                isMain = true,
                Alternative = plant.Name,
                Plant = plant
            };
            PlantImage hover = new PlantImage
            {
                Name = await plant.HoverPhoto.FileCreate(_env.WebRootPath, "assets/images/website-images"),
                isMain = null,
                Alternative = plant.Name,
                Plant = plant
            };
            return RedirectToAction(nameof(Index));
        }
    }
}
