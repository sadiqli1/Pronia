using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWork_07_13_2022.DAL;
using Microsoft.AspNetCore.Mvc;
using Pronia.Models;

namespace Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Category> model = _context.Categories.OrderByDescending(x => x.Id).ToList();
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Category category)
        {
            if(!ModelState.IsValid) return View();
            Category existed = _context.Categories.FirstOrDefault(x => x.Name.ToLower().Trim() == category.Name.ToLower().Trim());
            if(existed != null)
            {
                ModelState.AddModelError("Name", "You can not duplicate category name");
                return View();
            }
            _context.Categories.Add(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Update(int? id)
        {
            if(id is null || id == 0) return NotFound();
            Category category = _context.Categories.FirstOrDefault(c => c.Id == id);
            if(category == null) return NotFound();
            return View(category);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int? id, Category category)
        {
            if (id is null || id == 0) return NotFound();
            if (!ModelState.IsValid) return View();

            Category existed = _context.Categories.FirstOrDefault(c => c.Id == category.Id);
            if (existed == null) return NotFound();
            bool duplicate = _context.Categories.Any(c => c.Name.Trim().ToLower() == category.Name.Trim().ToLower());
            if(duplicate)
            {
                ModelState.AddModelError("Name", "You can not duplicate name");
                return View();
            }

            //existed.Name = category.Name;
            _context.Entry(existed).CurrentValues.SetValues(category);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id is null || id == 0) return NotFound();
            Category category = await _context.Categories.FindAsync(id);
            if(category == null) return NotFound();

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
