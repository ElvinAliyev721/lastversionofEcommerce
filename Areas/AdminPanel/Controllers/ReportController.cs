using Code.DAL;
using Code.Extentions;
using Code.Models;
using Microsoft.AspNetCore.Hosting;
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
    public class ReportController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public ReportController(AppDbContext context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Reports.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Report report)
        {
            if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid)
            {
                return View(report);
            }
            if (!report.Photo.CheckFile("image/"))
            {
                ModelState.AddModelError("Photo", "Olmaz !!!");
                return View(report);
            }
            if (report.Photo.CheckFileSize(300))
            {
                ModelState.AddModelError("Photo", "Daha az olchulu yukle");
                return View(report);
            }
            report.Image = await report.Photo.SaveFile(_env.WebRootPath, "img");
            await _context.Reports.AddAsync(report);
            report.ArticleDate = DateTime.Now.ToString("yyyy/MM/dd");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            Report report = await _context.Reports.FindAsync(id);
            if (report == null) return NotFound();
            return View(report);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            Report report = await _context.Reports.FindAsync(id);
            if (report == null) return NotFound();
            return View(report);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteReport(int? id)
        {
            if (id == null) return NotFound();
            Report report = await _context.Reports.FindAsync(id);
            if (report == null) return NotFound();

            string FilePath = _env.WebRootPath;

            Helper.Helper.DeleteFile(report.Image, "img", FilePath);
            _context.Reports.Remove(report);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return NotFound();
            Report report = await _context.Reports.FindAsync(id);
            if (report == null) return NotFound();
            return View(report);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int? id, Report report)
        {
            if (id == null) return NotFound();
            Report dbreport = await _context.Reports.FindAsync(id);
            if (dbreport == null) return NotFound();

            if (ModelState["Photo"].ValidationState == ModelValidationState.Invalid)
            {
                return View();
            }

            if (!report.Photo.CheckFile("image/"))
            {
                ModelState.AddModelError("Photo", "Dogru tipde fayl daxil edin");
                return View();
            }

            if (report.Photo.CheckFileSize(200))
            {
                ModelState.AddModelError("Photo", "daha az olculu fayl daxil edin");
                return View();
            }

            Helper.Helper.DeleteFile(dbreport.Image, "img", _env.WebRootPath);

            string fileName = await report.Photo.SaveFile(_env.WebRootPath, "img");

            dbreport.Image = fileName;
            dbreport.Name = report.Name;
            dbreport.Description = report.Description;
            dbreport.ArticleDate = DateTime.Now.ToString("yyyy/MM/dd");
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
