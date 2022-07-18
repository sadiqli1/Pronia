using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWork_07_13_2022.DAL;
using HomeWork_07_13_2022.Models;
using Microsoft.AspNetCore.Mvc;

namespace Pronia.Areas.ProniaAdmin.Controllers
{
    [Area("ProniaAdmin")]
    public class SliderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SliderController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Slider> sliders = _context.Sliders.ToList();
            return View(sliders);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if(!ModelState.IsValid) return View();

            return RedirectToAction(nameof(Index));
        }
    }
}
