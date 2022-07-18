using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWork_07_13_2022.DAL;
using Microsoft.AspNetCore.Mvc;
using Pronia.Models;

namespace Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class SizeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SizeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Size> sizes = _context.Sizes.ToList();
            return View(sizes);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Size size)
        {
            if(!ModelState.IsValid) return View();
            Size existed = _context.Sizes.FirstOrDefault(x => x.Name.Trim().ToLower() == size.Name.Trim().ToLower());
            if(existed != null)
            {
                ModelState.AddModelError("Name", "You can not duplicate size name");
                return View();
            }
            _context.Sizes.Add(size);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Update(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Size size = _context.Sizes.FirstOrDefault(y => y.Id == id);
            if (size == null) return NotFound();
            return View(size);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(int? id, Size size)
        {
            if (id is null || id == 0) return NotFound();
            if (!ModelState.IsValid) return View();

            Size existed = _context.Sizes.FirstOrDefault(c => c.Id == size.Id);
            if (existed == null) return NotFound();
            bool duplicate = _context.Sizes.Any(c => c.Name.Trim().ToLower() == size.Name.Trim().ToLower());
            if (duplicate)
            {
                ModelState.AddModelError("Name", "You can not duplicate name");
                return View();
            }

            //existed.Name = category.Name;
            _context.Entry(existed).CurrentValues.SetValues(size);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if(id is null || id == 0) return NotFound();
            Size size = await _context.Sizes.FindAsync(id);
            if (size == null) return NotFound();

            _context.Sizes.Remove(size);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
