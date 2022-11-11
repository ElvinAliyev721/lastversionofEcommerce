using Code.DAL;
using Code.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.Where(c => c.IsDeleted == false).ToListAsync());
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Category category = await _context.Categories.Include(c => c.Children).Include(c => c.Parent).FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted == false);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.MainCategories = await _context.Categories.Where(c => c.IsMain && c.IsDeleted == false).ToListAsync();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category, int? MainCategoryId)
        {
            ViewBag.MainCategories = await _context.Categories.Where(c => c.IsMain && c.IsDeleted == false).ToListAsync();
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            if (category.IsMain)
            {
                if (await _context.Categories.AnyAsync(c => c.IsMain && c.IsDeleted == false && c.Name.ToLower() == category.Name.ToLower()))
                {
                    ModelState.AddModelError("Name", "bele bir categoriya var Main olaraq");
                    return View(category);
                }
            }
            else
            {
                if (MainCategoryId == null)
                {
                    ModelState.AddModelError("", "Main Category movcud deyil");
                    return View(category);
                }
                Category mainCategory = await _context.Categories.Include(c => c.Children).FirstOrDefaultAsync(c => c.IsMain && c.IsDeleted == false && c.Id == MainCategoryId);

                if (mainCategory == null)
                {
                    return NotFound();
                }

                if (mainCategory.Children.Any(c => c.IsMain == false && c.IsDeleted == false && c.Name.ToLower() == category.Name.ToLower()))
                {
                    ModelState.AddModelError("Name", $"{mainCategory.Name} main categoriyasinin {category.Name} adli bir child categoriyasi var");
                    return View(category);
                }


                category.ParentId = MainCategoryId;
            }
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            Category category = await _context.Categories
                .Include(c => c.Children)
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted == false);
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (id == null) return NotFound();

            Category category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id && c.IsDeleted == false);

            if (category == null) return NotFound();


            if (category.IsMain)
            {
                List<Category> children = await _context.Categories.Where(c => c.ParentId == id).ToListAsync();

                if (children.Count > 0)
                {
                    foreach (Category c in children)
                    {
                        //c.IsDeleted = true;
                        _context.Categories.Remove(c);
                    }
                }

                //category.IsDeleted = true;
                _context.Categories.Remove(category);

            }
            else
            {
                //category.IsDeleted = true;
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();

            Category category = await _context.Categories.Include(c => c.Children).Include(c => c.Parent).FirstOrDefaultAsync(c => c.IsDeleted == false && c.Id == id);

            if (category == null) return NotFound();
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id , Category category)
        {
            if (id == null) return NotFound();

            Category dbcategory = await _context.Categories.Include(c => c.Parent).Include(c => c.Children).FirstOrDefaultAsync(c => c.IsDeleted == false && c.Id == id);

            if (dbcategory == null) return NotFound();


            if (!ModelState.IsValid)
            {
                return View(category);
            }


            if (await _context.Categories.AnyAsync(c => c.Name.ToLower() == category.Name.ToLower() && c.ParentId == category.ParentId && c.Id != id))
            {
                ModelState.AddModelError("Name", "Bu ad artiq movcuddur");
                return View(category);
            }
            dbcategory.Name = category.Name;
            dbcategory.Description = category.Description;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
