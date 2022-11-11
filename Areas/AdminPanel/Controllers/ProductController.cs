using Code.DAL;
using Code.Extentions;
using Code.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.GroupOfProducts
                .Include(g=>g.ProductImages)
                .Include(g=>g.Products).Where(g=>g.IsDeleted==false).ToListAsync()
                );
        }
        public async Task<IActionResult> GetChildCategory(int? mainCategoryId)
        {
            if (mainCategoryId == null) return NotFound();
            Category mainCategory = await _context.Categories
                .Include(c => c.Children)
                .FirstOrDefaultAsync(c => c.IsMain && c.IsDeleted == false && c.Id == mainCategoryId);
            if (mainCategory == null) return NotFound();
            return PartialView("_ChildCategoryPartial", mainCategory.Children.Where(c => c.IsDeleted == false));
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.MainCategories = await _context.Categories.Where(c => c.IsDeleted == false && c.IsMain).ToListAsync();
            int parentId = _context.Categories.Where(c => c.IsMain && c.IsDeleted == false).FirstOrDefault().Id;
            ViewBag.ChildCategories = await _context.Categories.Where(c => c.IsDeleted == false && c.IsMain == false && c.ParentId == parentId).ToListAsync();

            ViewBag.DbGroupNames = await _context.GroupOfProducts.Where(p => p.IsDeleted == false).ToListAsync();

            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();

            ViewBag.HasGroup = _context.GroupOfProducts.Count();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string Name, string Description, int? Price, IFormFile[] Photos, Product product, int? MainCategoryId, int? ChildCategoryId, int? ColorId, int? SizeId, int? GroupId)
        {
            ViewBag.MainCategories = await _context.Categories.Where(c => c.IsDeleted == false && c.IsMain).ToListAsync();
            int parentId = _context.Categories.Where(c => c.IsMain && c.IsDeleted == false).FirstOrDefault().Id;
            ViewBag.ChildCategories = await _context.Categories.Where(c => c.IsDeleted == false && c.IsMain == false && c.ParentId == parentId).ToListAsync();

            ViewBag.DbGroupNames = await _context.GroupOfProducts.Where(p => p.IsDeleted == false).ToListAsync();

            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();

            ViewBag.HasGroup = _context.GroupOfProducts.Count();

            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "dogru daxil edin");
                return View(product);
            }
            if (product.Count < 0 && product.Count > 50)
            {
                ModelState.AddModelError("Count", "bele bir say daxil etmeye haqqiniz yoxdur");
                return View(product);
            }
            if (product.IsSame)
            {

                #region LastForSameMain
                if (GroupId == null)
                {
                    return View();
                }
                GroupOfProduct dbGroup = await _context.GroupOfProducts.FirstOrDefaultAsync(g => g.Id == GroupId);
                if (dbGroup == null)
                {
                    return View();
                }
                Product fromGroup = await _context.Products.Include(p => p.ProductCategories).ThenInclude(p => p.Category).FirstOrDefaultAsync(p => p.GroupOfProductId == GroupId && p.IsDeleted == false);
                if (fromGroup == null)
                {
                    return View();
                }
                List<ProductSize> productSizes = new List<ProductSize>
                        {
                            new ProductSize {SizeId=(int)SizeId,ProductId=product.Id}
                        };
                List<ProductColor> productColors = new List<ProductColor>
                        {
                            new ProductColor{ColorId=(int)ColorId}
                        };
                List<ProductCategory> productCategor = new List<ProductCategory> {
                    new ProductCategory{CategoryId=fromGroup.ProductCategories.ElementAt(0).CategoryId}
                };
                if (fromGroup.ProductCategories.Count > 1)
                {
                    productCategor.Add(new ProductCategory { CategoryId = fromGroup.ProductCategories.ElementAt(1).CategoryId });
                }
                product.GroupOfProductId = (int)GroupId;
                product.ProductCategories = productCategor;
                product.ProductColors = productColors;
                product.ProductSizes = productSizes;


                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
                #endregion



            }
            else
            {
                if (MainCategoryId == null)
                {
                    ModelState.AddModelError("", "Main Category secilmelidi");
                    return View();
                }

                if (!(await _context.Categories.AnyAsync(c => c.IsMain && c.IsDeleted == false && c.Id == MainCategoryId)))
                {
                    ModelState.AddModelError("", "Dogru daxil edin");
                    return View();
                }

                GroupOfProduct Newgroup = new GroupOfProduct { };
                if (await _context.GroupOfProducts.AnyAsync(g => g.Name.ToLower() == Name.ToLower()))
                {
                    ModelState.AddModelError("", "Bele bir Product qrupu var");
                    return View();
                }
                Newgroup.Name = Name;
                Newgroup.Description = Description;
                Newgroup.Price = (int)Price;
                Newgroup.Count = product.Count;

                if (Photos == null)
                {
                    ModelState.AddModelError("", "Shekil mutleq secilmelidi");
                    return View();
                }
                List<ProductImage> productImages = new List<ProductImage>();

                foreach (IFormFile photo in Photos)
                {
                    if (!photo.CheckFile("image/"))
                    {
                        ModelState.AddModelError("Photo", "Duzgun Shakil Tipi Sec");
                        return View();
                    }

                    if (photo.CheckFileSize(300))
                    {
                        ModelState.AddModelError("Photo", "Seklin Maksimum 300 kb Ola Biler");
                        return View();
                    }

                    ProductImage productImage = new ProductImage
                    {
                        Image = await photo.SaveFile(_env.WebRootPath, "img")
                    };

                    productImages.Add(productImage);
                }


                List<ProductCategory> productCategories = new List<ProductCategory>
                        {
                            new ProductCategory {CategoryId=(int)MainCategoryId}
                        };
                if (ChildCategoryId != null)
                {
                    productCategories.Add(new ProductCategory { CategoryId = (int)ChildCategoryId });
                }

                List<ProductSize> productSizes = new List<ProductSize>
                        {
                            new ProductSize {SizeId=(int)SizeId,ProductId=product.Id}
                        };
                List<ProductColor> productColors = new List<ProductColor>
                        {
                            new ProductColor{ColorId=(int)ColorId}
                        };

                List<Product> products = new List<Product> {
                    new Product{ Count=product.Count , ProductColors= productColors , ProductCategories = productCategories , ProductSizes = productSizes }
                };
                Newgroup.ProductImages = productImages;
                Newgroup.Products = products;
                await _context.GroupOfProducts.AddAsync(Newgroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> GoProducts(int? id)
        {
            List<Product> products = await _context.Products
                .Include(p => p.GroupOfProduct)
                .Include(p => p.ProductSizes).ThenInclude(p => p.Size)
                .Include(p => p.ProductColors).ThenInclude(p => p.Color)
                .Include(p => p.ProductCategories).ThenInclude(p => p.Category)
                .Where(p => p.GroupOfProductId == id && p.IsDeleted == false)
                .ToListAsync();
            return View(products);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null) return NotFound();
            ProductImage productImage = await _context.ProductImages.FindAsync(id);
            if (productImage == null) return NotFound();
            GroupOfProduct product = await _context.GroupOfProducts
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(c => c.IsDeleted == false && c.Id == productImage.GroupId);

            if (product.ProductImages.Count() == 1)
            {
                TempData["ImageError"] = "Minimum Bir Shekil Olmalidi";
                return RedirectToAction("Update", new { id = productImage.GroupId });
            }

            Helper.Helper.DeleteFile(productImage.Image, _env.WebRootPath, "img");

            _context.ProductImages.Remove(productImage);
            await _context.SaveChangesAsync();
            return RedirectToAction("UpdateGroup", new { id = productImage.GroupId });
        }
        public async Task<IActionResult> UpdateGroup(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            GroupOfProduct group = await _context.GroupOfProducts
                .Include(g => g.Products)
                .Include(g => g.ProductImages)
                .FirstOrDefaultAsync(g => g.Id == id && g.IsDeleted == false);
            return View(group);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateGroup(int? id,GroupOfProduct group)
        {
            if (id == null)
            {
                return NotFound();
            }
            GroupOfProduct dbgroup = await _context.GroupOfProducts
                .Include(g => g.Products)
                .Include(g => g.ProductImages)
                .FirstOrDefaultAsync(g => g.Id == id && g.IsDeleted == false);

            if (dbgroup == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(dbgroup);
            }
            foreach (IFormFile photo in group.Photos)
            {
                if (!photo.CheckFile("image/"))
                {
                    ModelState.AddModelError("Photo", "Made By Parvin: Decellik Eleme Duzgun Shakil Tipi Sec");
                    return View();
                }

                if (photo.CheckFileSize(300))
                {
                    ModelState.AddModelError("Photo", "Made By Parvin: Decellik Eleme Seklin Maksimum 300 kb Ola Biler");
                    return View();
                }

                ProductImage productImage = new ProductImage
                {
                    Image = await photo.SaveFile(_env.WebRootPath, "img")
                };

                dbgroup.ProductImages.Add(productImage);
            }
            dbgroup.Name = group.Name;
            dbgroup.Description = group.Description;
            dbgroup.Price = group.Price;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DeleteGroup(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            GroupOfProduct dbgroup = await _context.GroupOfProducts
                .Include(g => g.ProductImages)
                .Include(g => g.Products).ThenInclude(g => g.ProductCategories)
                .FirstOrDefaultAsync(g => g.Id == id && g.IsDeleted == false);
            if (dbgroup == null)
            {
                return NotFound();
            }
            return View(dbgroup);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("DeleteGroup")]
        public async Task<IActionResult> DeleteFullGroup(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            GroupOfProduct dbgroup = await _context.GroupOfProducts
                .Include(g => g.ProductImages)
                .Include(g => g.Products).ThenInclude(g => g.ProductCategories)
                .FirstOrDefaultAsync(g => g.Id == id && g.IsDeleted == false);
            if (dbgroup == null)
            {
                return NotFound();
            }
            foreach (Product product in dbgroup.Products)
            {
                product.IsDeleted = true;
                _context.Products.Remove(product);
            }
            dbgroup.IsDeleted = true;
            _context.GroupOfProducts.Remove(dbgroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product dbproduct = await _context.Products
                .Include(p => p.ProductColors).ThenInclude(p => p.Color)
                .Include(p => p.ProductSizes).ThenInclude(s => s.Size)
                .Include(p => p.ProductCategories).ThenInclude(c => c.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
            if (dbproduct == null)
            {
                return NotFound();
            }
            return View(dbproduct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Product dbproduct = await _context.Products
                .Include(p => p.ProductColors).ThenInclude(p => p.Color)
                .Include(p => p.ProductSizes).ThenInclude(s => s.Size)
                .Include(p => p.ProductCategories).ThenInclude(c => c.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
            if (dbproduct == null)
            {
                return NotFound();
            }

            GroupOfProduct dbGroup = await _context.GroupOfProducts
                .Include(g => g.Products)
                .Include(g => g.ProductImages).FirstOrDefaultAsync(g => g.Id == dbproduct.GroupOfProductId);



            if (dbGroup.Products.Count() == 1)
            {
                dbproduct.IsDeleted = true;
                _context.Products.Remove(dbproduct);
                _context.GroupOfProducts.Remove(dbGroup);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            Product dbproduct = await _context.Products
                .Include(p => p.ProductColors).ThenInclude(p => p.Color)
                .Include(p => p.ProductSizes).ThenInclude(s => s.Size)
                .Include(p => p.ProductCategories).ThenInclude(c => c.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
            if (dbproduct == null)
            {
                return NotFound();
            }

            return View(dbproduct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Product product, int? SizeId, int? ColorId)
        {
            ViewBag.Sizes = await _context.Sizes.ToListAsync();
            ViewBag.Colors = await _context.Colors.ToListAsync();
            if (id == null)
            {
                return NotFound();
            }
            Product dbproduct = await _context.Products
                .Include(p => p.ProductColors).ThenInclude(p => p.Color)
                .Include(p => p.ProductSizes).ThenInclude(s => s.Size)
                .Include(p => p.ProductCategories).ThenInclude(c => c.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
            if (dbproduct == null)
            {
                return NotFound();
            }
            dbproduct.ProductColors.ElementAt(0).ColorId = (int)ColorId;
            dbproduct.ProductSizes.ElementAt(0).SizeId = (int)SizeId;
            dbproduct.Count = product.Count;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
