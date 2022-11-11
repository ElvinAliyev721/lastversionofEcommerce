using Code.DAL;
using Code.Extentions;
using Code.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sliders.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            Slider slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            return View(slider);
        }
        public IActionResult Create()
        {
            if (_context.Sliders.Count() > 5)
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (ModelState["Photos"].ValidationState == ModelValidationState.Invalid)
            {
                return RedirectToAction(nameof(Index));
            }


            int counttotal = 5 - (_context.Sliders.Count());
            if (counttotal < slider.Photos.Length)
            {
                ModelState.AddModelError("Photos", "Artiq Fayl Olmaz");
                return View(slider);
            }

            foreach (IFormFile item in slider.Photos)
            {
                if (_context.Sliders.Count() > 5)
                {
                    return RedirectToAction(nameof(Index));
                }
                if (ModelState["Photos"].ValidationState == ModelValidationState.Invalid)
                {
                    return View();
                }
                if (!item.CheckFile("image/"))
                {
                    ModelState.AddModelError("Photos", "Shekil daxil edin");
                    return View();
                }
                if (item.CheckFileSize(200))
                {
                    ModelState.AddModelError("Photos", "Zehmet olmasa daha az olchulu fayl yukluyun");
                    return View();
                }
                Slider newone = new Slider
                {
                    Image = await item.SaveFile(_env.WebRootPath, "img")
                };

                await _context.Sliders.AddAsync(newone);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Slider slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteSlider(int? id)
        {
            if (id == null) return NotFound();
            Slider slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();

            string FilePath = _env.WebRootPath;

            Helper.Helper.DeleteFile(slider.Image, "img", FilePath);
            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            Slider slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            return View(slider);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id,Slider slider)
        {
            if (id == null) return NotFound();
            Slider dbslider = await _context.Sliders.FindAsync(id);
            if (dbslider == null) return NotFound();

            if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid)
            {
                return View();
            }

            if (!slider.Photo.CheckFile("image/"))
            {
                ModelState.AddModelError("Photo", "Dogru tipde fayl daxil edin");
                return View();
            }

            if (slider.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "daha az olculu fayl daxil edin");
                return View();
            }

            Helper.Helper.DeleteFile(dbslider.Image, "img", _env.WebRootPath);

            string fileName = await slider.Photo.SaveFile(_env.WebRootPath, "img");

            dbslider.Image = fileName;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
