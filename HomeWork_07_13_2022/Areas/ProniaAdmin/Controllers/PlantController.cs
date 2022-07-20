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
            plant.PlantImages = new List<PlantImage>();
            TempData["FileName"] = "";
            List<IFormFile> removeable = new List<IFormFile>();
            foreach (IFormFile photo in plant.Photos.ToList())
            {
                if (!photo.ImageisOkay(2))
                {
                    removeable.Add(photo);
                    TempData["FileName"] += photo.FileName + ",";
                    continue;
                }
                PlantImage another = new PlantImage()
                {
                    Name = await photo.FileCreate(_env.WebRootPath, "assets/images/website-images"),
                    isMain = false,
                    Alternative = photo.Name,
                    Plant = plant
                };
                plant.PlantImages.Add(another);
            }

            plant.Photos.RemoveAll(p => removeable.Any(r => r.FileName == p.FileName));

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
            plant.PlantImages.Add(main);
            plant.PlantImages.Add(hover);

            plant.PlantCategories = new List<PlantCategory>();
            foreach (int id in plant.CategoryIds)
            {
                PlantCategory category = new PlantCategory()
                {
                    CategoryId = id,
                    Plant = plant
                };
                plant.PlantCategories.Add(category);
            }
            await _context.Plants.AddAsync(plant);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Update(int? id)
        {
            if (id is null || id == 0) return NotFound();
            Plant model = await _context.Plants
                .Include(p => p.PlantImages)
                .Include(p => p.PlantCategories).ThenInclude(c => c.Category)
                .Include(p => p.PlantInformation)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (model == null) return NotFound();
            return View(model);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, Plant plant)
        {
            if (id is null || id == 0) return NotFound();
            Plant model = await _context.Plants
                .Include(p => p.PlantImages)
                .Include(p => p.PlantCategories).ThenInclude(c => c.Category)
                .Include(p => p.PlantInformation)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (!ModelState.IsValid) return View(model);

            if (plant.Photos == null)
            {

            }
            return Json(plant.Photos == null);
            return View(plant);
        }
        public IActionResult Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();
            Plant existed = _context.Plants.Include(p => p.PlantImages).Include(p => p.PlantCategories).ThenInclude(p => p.Category).FirstOrDefault(p => p.Id == id);
            if(existed == null) return NotFound();
            existed.PlantImages = new List<PlantImage>();

            foreach (PlantImage item in existed.PlantImages)
            {
                FileValidator.FileDelete(_env.WebRootPath, "assets/images/website-images", item.Name);
            }

            _context.Plants.Remove(existed);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
