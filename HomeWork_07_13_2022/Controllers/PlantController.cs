using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeWork_07_13_2022.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pronia.Models;
using Pronia.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            ViewBag.Plant = await _context.Plants.Include(p => p.PlantImages).Include(p => p.PlantCategories).ThenInclude(c => c.Category).ToListAsync();
            return View(plant);
        }
        public async Task<IActionResult> GetDetail(int? id)
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
            return PartialView("_getdetail", plant);
        }
        public async Task<IActionResult> Partial()
        {
            List<Plant> plants = await _context.Plants.Include(p => p.PlantImages).ToListAsync();
            return PartialView("_PlantsPartialView", plants);
        }

        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id == null || id == 0) return NotFound();
            Plant plant = await _context.Plants.FirstOrDefaultAsync(p => p.Id == id);
            if(plant == null) return NotFound();
            string basketstr = HttpContext.Request.Cookies["Basket"];

            BasketVM basket;
            if (string.IsNullOrEmpty(basketstr))
            {
                basket = new BasketVM();
                BasketCookieItemVM basketcookieitemvm = new BasketCookieItemVM()
                {
                    Id = plant.Id,
                    Quantity = 1
                };
                basket.BasketCookieItemVMs = new List<BasketCookieItemVM>();
                basket.BasketCookieItemVMs.Add(basketcookieitemvm);
                basket.TotalPrice = plant.Price;
            }
            else
            {
                basket = JsonConvert.DeserializeObject<BasketVM>(basketstr);
                BasketCookieItemVM existed = basket.BasketCookieItemVMs.FirstOrDefault(x => x.Id == id);
                if (existed == null)
                {
                    BasketCookieItemVM basketcookieitemvm = new BasketCookieItemVM()
                    {
                        Id = plant.Id,
                        Quantity = 1
                    };
                    basket.BasketCookieItemVMs.Add(basketcookieitemvm);
                    basket.TotalPrice += plant.Price;
                }
                else
                {
                    basket.TotalPrice += plant.Price;
                    existed.Quantity++;
                }
            }

            basketstr = JsonConvert.SerializeObject(basket);
            HttpContext.Response.Cookies.Append("Basket", basketstr);

            return RedirectToAction("Index", "Home");
        }
        public IActionResult ShowBasket()
        {
            if (HttpContext.Request.Cookies["Basket"] == null) return NotFound();
            BasketVM basket = JsonConvert.DeserializeObject<BasketVM>(HttpContext.Request.Cookies["Basket"]);
            return Json(basket);
        }
        public IActionResult RemoveBasketItem(int? id)
        {
            Plant plant = _context.Plants.FirstOrDefault(p => p.Id == id);
            BasketVM basket = JsonConvert.DeserializeObject<BasketVM>(HttpContext.Request.Cookies["Basket"]);
            if (basket != null)
            {
                foreach (BasketCookieItemVM item in basket.BasketCookieItemVMs)
                {
                    if (item.Id == id)
                    {
                        basket.TotalPrice -= (plant.Price * basket.BasketCookieItemVMs.FirstOrDefault(x => x.Id == id).Quantity);
                        basket.BasketCookieItemVMs.Remove(item);
                        break;
                    }
                }
            }
            string basketstr = JsonConvert.SerializeObject(basket);
            HttpContext.Response.Cookies.Append("Basket",basketstr);
            return RedirectToAction("Index", "Home");
        }
    }
}