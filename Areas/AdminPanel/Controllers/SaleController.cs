using Code.DAL;
using Code.Models;
using Code.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SaleController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public SaleController(AppDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            List<SaleVM> sales = await _context.Sales
                .Include(s => s.AppUser)
                .Include(s => s.SaleProducts).ThenInclude(sp => sp.Product)
                .ThenInclude(p=>p.GroupOfProduct)
                .Select(x => new SaleVM { 
                    Id=x.Id,
                    Date=x.Date,
                    Email=x.AppUser.Email,
                    Total=x.Total,
                    SaleProductVMs=x.SaleProducts.Select(xs=>new SaleProductVM
                    {
                        Id=xs.Id,
                        Count=xs.Count,
                        Price=xs.Price,
                        Title=xs.Product.GroupOfProduct.Name,
                        //Color=xs.Product.ProductColors.ElementAt(0).Color.Name,
                        //Size=xs.Product.ProductSizes.ElementAt(0).Size.Measure
                    }).ToList()
                }).ToListAsync();
            return View(sales);
        }
    }
}
