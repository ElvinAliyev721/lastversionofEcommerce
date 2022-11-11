using Code.DAL;
using Code.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.ViewComponents
{
    public class HeaderViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;
        public HeaderViewComponent(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Category> categories = await _context.Categories.Include(c => c.Children).Where(c => c.IsDeleted == false).ToListAsync();
            return View(await Task.FromResult(categories));
        }
    }
}
