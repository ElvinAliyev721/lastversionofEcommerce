using Code.DAL;
using Code.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Controllers
{
    public class SearchController : Controller
    {
        private readonly AppDbContext _context;
        public SearchController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> Search(string search)
        {
            List<Product> products = await _context.Products
                .Include(p=>p.ProductColors).ThenInclude(p=>p.Color)
                .Include(p=>p.ProductSizes).ThenInclude(p=>p.Size)
                .Include(p=>p.GroupOfProduct).ThenInclude(p=>p.ProductImages)
                .Where(p=>p.GroupOfProduct.Name.Contains(search))
                .OrderByDescending(p=>p.Id)
                .ToListAsync();
            return PartialView("_SeachPartial",products);
        }
    }
}
