using Code.DAL;
using Code.Models;
using Code.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public HomeController(AppDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            HomeVM model = new HomeVM
            {
                Categories = await _context.Categories.ToListAsync(),

                Sliders = await _context.Sliders.ToListAsync(),

                GroupOfProducts = await _context.GroupOfProducts
                .Include(g => g.ProductImages)
                .Include(g => g.Products)
                .ThenInclude(p => p.ProductCategories).ThenInclude(pc => pc.Category)
                .ToListAsync(),
                Products = await _context.Products
                .Include(p=>p.GroupOfProduct).ThenInclude(p=>p.ProductImages)
                .Include(p=>p.ProductCategories).ThenInclude(p=>p.Category)
                .Include(p=>p.ProductColors).ThenInclude(p=>p.Color)
                .Include(p=>p.ProductSizes).ThenInclude(p=>p.Size)
                .ToListAsync()
            };
            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }
        public async Task<IActionResult> News()
        {
            return View(await _context.Reports.ToListAsync());
        }
        public async Task<IActionResult> DetailNews(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Report report = await _context.Reports.FirstOrDefaultAsync(c => c.Id == id);
            if (report == null)
            {
                return NotFound();
            }
            return View(report);
        }
        public IActionResult ReturnInformation()
        {
            return View();
        }
        public IActionResult TermConditions()
        {
            return View();
        }
        public IActionResult SizeGuide()
        {
            return View();
        }
        public IActionResult Delivery()
        {
            return View();
        }
        public IActionResult Policy()
        {
            return View();
        }

        //Basket
        public async Task<IActionResult> AddToBasket(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product product = await _context.Products
                .Include(p => p.GroupOfProduct).ThenInclude(g => g.ProductImages)
                .Include(p => p.ProductColors).ThenInclude(p => p.Color)
                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            List<BasketVM> Basketproducts;
            if (Request.Cookies["basket"] == null)
            {
                Basketproducts = new List<BasketVM>();
            }
            else
            {
                Basketproducts = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }

            BasketVM existbasket = Basketproducts.Find(b => b.Id == id);
            if (existbasket != null)
            {
                existbasket.BasketCount++;
            }
            else
            {
                BasketVM newbasketProduct = new BasketVM
                {
                    Id = product.Id,
                    BasketCount = 1
                };

                Basketproducts.Add(newbasketProduct);
            }



            string basketProducts = JsonConvert.SerializeObject(Basketproducts, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            Response.Cookies.Append("basket", basketProducts, new CookieOptions { MaxAge = TimeSpan.FromDays(14) });

            return RedirectToAction(nameof(Basket));
        }

        public async Task<IActionResult> Basket()
        {
            string basket = Request.Cookies["basket"];
            List<BasketVM> products;
            if (basket!=null)
            {
                products= JsonConvert.DeserializeObject<List<BasketVM>>(basket);
            }
            else
            {
                products = new List<BasketVM>();
            }
            foreach (BasketVM basketVM in products)
            {
                Product product = await _context.Products
                    .Include(p => p.GroupOfProduct).ThenInclude(p => p.ProductImages)
                    .Include(p=>p.ProductColors).ThenInclude(p=>p.Color)
                    .Include(p=>p.ProductSizes).ThenInclude(p=>p.Size)
                    .FirstOrDefaultAsync(p => p.Id == basketVM.Id);
                basketVM.Name = product.GroupOfProduct.Name;
                basketVM.Color = product.ProductColors.ElementAt(0).Color.Name;
                basketVM.Size = product.ProductSizes.ElementAt(0).Size.Measure;
                basketVM.Image = product.GroupOfProduct.ProductImages.ElementAt(0).Image;
                basketVM.Description = product.GroupOfProduct.Description;
                basketVM.Price = product.GroupOfProduct.Price;
            }
            return View(products);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Basket")]
        public async Task<IActionResult> BasketPost()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login","Account");
            }
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<BasketVM> basketVMs;
            if (Request.Cookies["basket"] !=null)
            {
                basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            else
            {
                return RedirectToAction("Index");
            }

            List<SaleProduct> saleProducts = new List<SaleProduct> { };
            double total = 0;
            foreach (BasketVM basket in basketVMs)
            {
                Product product = await _context.Products
                    .Include(p=>p.ProductColors).ThenInclude(p=>p.Color)
                    .Include(p=>p.ProductSizes).ThenInclude(p=>p.Size)
                    .Include(p=>p.GroupOfProduct)
                    .FirstOrDefaultAsync(p=>p.Id==basket.Id);
                if (product.Count<basket.BasketCount)
                {
                    TempData["error"] = $"{product.GroupOfProduct.Name} mehsulunun {product.ProductColors.ElementAt(0).Color.Name} renginden {product.ProductSizes.ElementAt(0).Size.Measure} olcusunden {product.Count} qeder qalib";
                    return RedirectToAction(nameof(Basket));
                }

                product.Count -= basket.BasketCount;

                SaleProduct saleProduct = new SaleProduct
                {
                    Price=product.GroupOfProduct.Price,
                    Count=basket.BasketCount,
                    Color=product.ProductColors.ElementAt(0).Color.Name,
                    Size=product.ProductSizes.ElementAt(0).Size.Measure,
                    ProductId=product.Id
                };
                saleProducts.Add(saleProduct);
                total += saleProduct.Price * saleProduct.Count;
            }
            Sale sale = new Sale
            {
                AppUserId=user.Id,
                Date=DateTime.UtcNow.AddHours(4),
                SaleProducts=saleProducts,
                Total=total

            };
            TempData["success"] = "Succuss ... You are wellcome";
            await _context.Sales.AddAsync(sale);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Basket));
        }
        public async Task<IActionResult> DeleteFromBasket(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            Product product = await _context.Products.FindAsync(id);
            if (product==null)
            {
                return NotFound();
            }
            List<BasketVM> currentbasket;
            if (Request.Cookies["basket"]!=null)
            {
                currentbasket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            else
            {
                return NotFound();
            }
            BasketVM basketVM = currentbasket.Find(p=>p.Id==id);
            if (basketVM==null)
            {
                return NotFound();
            }
            else
            {
                currentbasket.Remove(basketVM);
            }
            string cookies = JsonConvert.SerializeObject(currentbasket);
            Response.Cookies.Append("basket", cookies, new CookieOptions {MaxAge=TimeSpan.FromDays(14) });
            return RedirectToAction(nameof(Basket));
        }
        public async Task<IActionResult> Minuse(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            Product product = await _context.Products.FindAsync(id);
            if (product==null)
            {
                return NotFound();
            }
            List<BasketVM> currentbasket;
            if (Request.Cookies["basket"]==null)
            {
                return NotFound();
            }
            else
            {
                currentbasket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            foreach (BasketVM basket in currentbasket)
            {
                if (basket.Id==id)
                {
                    basket.BasketCount--;
                    if (basket.BasketCount==0)
                    {
                        currentbasket.Remove(basket);
                        string cookiesforremoveditem = JsonConvert.SerializeObject(currentbasket);
                        Response.Cookies.Append("basket",cookiesforremoveditem,new CookieOptions { MaxAge=TimeSpan.FromDays(14)});
                        return RedirectToAction(nameof(Basket));
                    }
                }
            }
            string cookies = JsonConvert.SerializeObject(currentbasket);
            Response.Cookies.Append("basket", cookies,new CookieOptions{MaxAge=TimeSpan.FromDays(14) });
            return RedirectToAction(nameof(Basket));
        }
        public async Task<IActionResult> Plus(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            List<BasketVM> currentbasket;
            if (Request.Cookies["basket"] == null)
            {
                return NotFound();
            }
            else
            {
                currentbasket = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies["basket"]);
            }
            foreach (BasketVM basket in currentbasket)
            {
                if (basket.Id == id)
                {
                    basket.BasketCount++;
                }
            }
            string cookies = JsonConvert.SerializeObject(currentbasket);
            Response.Cookies.Append("basket", cookies, new CookieOptions { MaxAge = TimeSpan.FromDays(14) });
            return RedirectToAction(nameof(Basket));
        }
    }
}
