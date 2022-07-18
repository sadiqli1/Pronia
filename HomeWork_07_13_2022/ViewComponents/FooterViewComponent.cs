using System.Collections.Generic;
using System.Threading.Tasks;
using HomeWork_07_13_2022.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pronia.Models;

namespace Pronia.ViewComponents
{
    public class FooterViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public FooterViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Settings> model = await _context.Settings.ToListAsync();

            return View(model);
        }
    }
}
