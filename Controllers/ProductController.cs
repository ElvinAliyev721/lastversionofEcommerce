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
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        public ProductController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int? id)
        {
            TempData["categoryId"] = id;
            Response.Cookies.Append("categoryId", id.ToString());
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            if (id==null)
            {
                return NotFound();
            }
            List<Product> products;
            if ((int)id==6)
            {
                products= await _context.Products
                .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                .Where(p=>p.IsDeleted==false)
                .OrderByDescending(p=>p.Id)
                .ToListAsync();
            }
            else
            {
                products = await _context.Products
                .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                .Where(p => p.IsDeleted == false && p.ProductCategories.Any(p => p.CategoryId == id))
                .ToListAsync();
            }
            
            return View(products);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            Product product = await _context.Products
                .Include(p => p.GroupOfProduct).ThenInclude(p => p.ProductImages)
                .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                .Include(p => p.ProductColors).ThenInclude(p => p.Color)
                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
            return View(product);
        }
        public async Task<IActionResult> LoadProducts(int? colorId, int? sizeId,int? sortId)
        {
            List<Product> coloredProducts = new List<Product> { };
            int? id = int.Parse(Request.Cookies["categoryId"]);
            if (id == null)
            {
                return NotFound();
            }

            if (colorId != null)
            {
                Response.Cookies.Append("ColorId", colorId.ToString());
            }

            if (sizeId != null)
            {
                Response.Cookies.Append("SizeId", sizeId.ToString());
            }
            //bool for category id equal to 6 
            if (colorId == null && sizeId == null)
            {
                if (id==6)
                {
                    coloredProducts = await _context.Products
                .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                .Where(p => p.IsDeleted == false)
                .ToListAsync();
                }
                else
                {
                    coloredProducts = await _context.Products
                .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                .Where(p => p.IsDeleted == false && p.ProductCategories.Any(p => p.CategoryId == id))
                .ToListAsync();
                }
                
            }
            else if (colorId != null && sizeId == null)
            {
                if (id==6)
                {
                    coloredProducts = await _context.Products
                .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                .Where(p => p.IsDeleted == false && p.ProductColors.Any(p => p.ColorId == colorId))
                .ToListAsync();
                }
                else
                {
                    coloredProducts = await _context.Products
                .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                .Where(p => p.IsDeleted == false && p.ProductCategories.Any(p => p.CategoryId == id) && p.ProductColors.Any(p => p.ColorId == colorId))
                .ToListAsync();
                }
                
            }
            else if (colorId == null && sizeId != null)
            {
                if (id==6)
                {
                    coloredProducts = await _context.Products
                .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                .Where(p => p.IsDeleted == false && p.ProductSizes.Any(p => p.SizeId == sizeId))
                .ToListAsync();
                }
                else
                {
                    coloredProducts = await _context.Products
                .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                .Where(p => p.IsDeleted == false && p.ProductCategories.Any(p => p.CategoryId == id) && p.ProductSizes.Any(p => p.SizeId == sizeId))
                .ToListAsync();
                }
                
            }
            if (sortId!=null)
            {
                switch (sortId)
                {
                    case 1:
                        coloredProducts = await _context.Products
                                .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                                .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                                .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                                .Where(p => p.IsDeleted == false && p.ProductCategories.Any(p => p.CategoryId == id))
                                .OrderByDescending(p => p.GroupOfProduct.Name)
                                .ToListAsync();
                        break;
                    case 2:
                        coloredProducts = await _context.Products
                                .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                                .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                                .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                                .Where(p => p.IsDeleted == false && p.ProductCategories.Any(p => p.CategoryId == id))
                                .OrderByDescending(p => p.GroupOfProduct.Name).Reverse()
                                .ToListAsync();
                        break;
                    case 3:
                        coloredProducts = await _context.Products
                                .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                                .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                                .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                                .Where(p => p.IsDeleted == false && p.ProductCategories.Any(p => p.CategoryId == id))
                                .OrderByDescending(p => p.GroupOfProduct.Price).Reverse()
                                .ToListAsync();
                        break;
                    case 4:
                        coloredProducts = await _context.Products
                                .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                                .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                                .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                                .Where(p => p.IsDeleted == false && p.ProductCategories.Any(p => p.CategoryId == id))
                                .OrderByDescending(p => p.GroupOfProduct.Price)
                                .ToListAsync();
                        break;
                    case 5:
                        coloredProducts = await _context.Products
                                .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                                .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                                .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                                .Where(p => p.IsDeleted == false && p.ProductCategories.Any(p => p.CategoryId == id))
                                .OrderByDescending(p => p.Id)
                                .ToListAsync();
                        break;
                    case 6:
                        coloredProducts = await _context.Products
                                .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                                .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                                .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                                .Where(p => p.IsDeleted == false && p.ProductCategories.Any(p => p.CategoryId == id))
                                .OrderByDescending(p => p.Id)
                                .ToListAsync();
                        break;
                    default:
                        coloredProducts = await _context.Products
                                .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                                .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                                .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                                .Where(p => p.IsDeleted == false && p.ProductCategories.Any(p => p.CategoryId == id))
                                .OrderByDescending(p => p.Id)
                                .ToListAsync();
                        break;
                }
            }
            return PartialView("_ProductPartial", coloredProducts);
        }
        public async Task<IActionResult> SortBy(int? sortId)
        {
            List<Product> products = new List<Product> { };
            if (sortId==null)
            {
                return NotFound();
            }
            int? id = int.Parse(Request.Cookies["categoryId"]);
            if (id == null)
            {
                return NotFound();
            }
            switch (sortId)
            {
                case 1:
                    products= await _context.Products
                            .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                            .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                            .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                            .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                            .Where(p => p.IsDeleted == false && p.ProductCategories.Any(p => p.CategoryId == id))
                            .OrderByDescending(p=>p.GroupOfProduct.Name)
                            .ToListAsync();
                    break;
                case 2:
                    products = await _context.Products
                            .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                            .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                            .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                            .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                            .Where(p => p.IsDeleted == false && p.ProductCategories.Any(p => p.CategoryId == id))
                            .OrderByDescending(p => p.GroupOfProduct.Name).Reverse()
                            .ToListAsync();
                    break;
                case 3:
                    products = await _context.Products
                            .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                            .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                            .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                            .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                            .Where(p => p.IsDeleted == false && p.ProductCategories.Any(p => p.CategoryId == id))
                            .OrderByDescending(p => p.GroupOfProduct.Price).Reverse()
                            .ToListAsync();
                    break;
                case 4:
                    products = await _context.Products
                            .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                            .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                            .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                            .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                            .Where(p => p.IsDeleted == false && p.ProductCategories.Any(p => p.CategoryId == id))
                            .OrderByDescending(p => p.GroupOfProduct.Price)
                            .ToListAsync();
                    break;
                case 5:
                    products = await _context.Products
                            .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                            .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                            .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                            .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                            .Where(p => p.IsDeleted == false && p.ProductCategories.Any(p => p.CategoryId == id))
                            .OrderByDescending(p => p.Id)
                            .ToListAsync();
                    break;
                case 6:
                    products = await _context.Products
                            .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                            .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                            .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                            .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                            .Where(p => p.IsDeleted == false && p.ProductCategories.Any(p => p.CategoryId == id))
                            .OrderByDescending(p => p.Id)
                            .ToListAsync();
                    break;
                default:
                    products = await _context.Products
                            .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                            .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                            .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                            .Include(p => p.ProductColors).ThenInclude(c => c.Color)
                            .Where(p => p.IsDeleted == false && p.ProductCategories.Any(p => p.CategoryId == id))
                            .OrderByDescending(p => p.Id)
                            .ToListAsync();
                    break;
            }
            

            return PartialView("_ProductPartial",products);
        }
    }
}
