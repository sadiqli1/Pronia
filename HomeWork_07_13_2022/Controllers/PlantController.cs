using System.Linq;
using System.Threading.Tasks;
using HomeWork_07_13_2022.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Models;

namespace Pronia.Controllers
{
    public class PlantController:Controller
    {
        private readonly ApplicationDbContext _context;

        public PlantController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Plant plant = await _context.Plants.Include(x => x.PlantImages)
                .Include(x => x.PlantInformation)
                .Include(x => x.PlantTags)
                .ThenInclude(x => x.Tag)
                .Include(x => x.PlantCategories)
                .ThenInclude(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
            if(plant == null) return NotFound();
            return View(plant);
        }
    }
}
