using System.Collections.Generic;
using System.Linq;
using HomeWork_07_13_2022.DAL;
using HomeWork_07_13_2022.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.ViewModels;

namespace HomeWork_07_13_2022.Controllers
{
    public class HomeController:Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            HomeVM model = new HomeVM()
            {
                Sliders = _context.Sliders.ToList(),
                Plants = _context.Plants.Include(p => p.PlantImages).ToList()
            };
            return View(model);
        }
    }
}
