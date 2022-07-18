using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWork_07_13_2022.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Models;

namespace Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class ColorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ColorController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<Color> colors = await _context.Colors.ToListAsync();
            return View(colors);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Color color)
        {
            if (!ModelState.IsValid) return View();
            Color existed = _context.Colors.FirstOrDefault(c => c.Name.Trim().ToLower() == color.Name.Trim().ToLower());
            if(existed != null)
            {
                ModelState.AddModelError("Name", "You can not duplicate color name");
                return View();
            }
            _context.Colors.Add(color);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if (id is null || id == 0) return NotFound();
            Color color = _context.Colors.FirstOrDefault(c => c.Id == id);
            if (color == null) return NotFound();
            return View(color);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int? id, Color color)
        {
            if (id is null || id == 0) return NotFound();
            if (!ModelState.IsValid) return View();

            Color existed = _context.Colors.FirstOrDefault(c => c.Id == color.Id);
            if (existed == null) return NotFound();
            bool duplicate = _context.Colors.Any(c => c.Name.Trim().ToLower() == color.Name.Trim().ToLower());
            if (duplicate)
            {
                ModelState.AddModelError("Name", "You can not duplicate name");
                return View();
            }

            //existed.Name = category.Name;
            _context.Entry(existed).CurrentValues.SetValues(color);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();
            Color color = await _context.Colors.FindAsync(id);
            if (color == null) return NotFound();

            _context.Colors.Remove(color);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
    }
}
