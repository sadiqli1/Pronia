using System.Collections.Generic;
using System.Linq;
using HomeWork_07_13_2022.DAL;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Pronia.Models;
using Pronia.ViewModels;

namespace Pronia.Service
{
    public class LayoutService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _http;

        public LayoutService(ApplicationDbContext context, IHttpContextAccessor http)
        {
            _context = context;
            _http = http;
        }
        public List<Settings> GetSetting()
        {
            List<Settings> settings = _context.Settings.ToList();
            return settings;
        }
        public LayoutBasketVM GetBasket()
        {
            string basketstr = _http.HttpContext.Request.Cookies["Basket"];
            if (!string.IsNullOrEmpty(basketstr))
            {
                BasketVM basket = JsonConvert.DeserializeObject<BasketVM>(basketstr);
                LayoutBasketVM layoutBasket = new LayoutBasketVM();
                layoutBasket.BasketItemVMs = new List<BasketItemVM>();
                foreach (BasketCookieItemVM cookie in basket.BasketCookieItemVMs)
                {
                    Plant existed = _context.Plants.FirstOrDefault(p => p.Id == cookie.Id);
                    if (existed == null)
                    {
                        basket.BasketCookieItemVMs.Remove(cookie);
                        continue;
                    }
                    BasketItemVM basketItem = new BasketItemVM()
                    {
                        Plant = existed,
                        Quantity = cookie.Quantity
                    };
                    layoutBasket.BasketItemVMs.Add(basketItem);
                }
                layoutBasket.TotalPrice = basket.TotalPrice;
                return layoutBasket;
            }
            return null;
        }
    }
}
