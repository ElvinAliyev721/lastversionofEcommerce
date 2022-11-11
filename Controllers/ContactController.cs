using Code.DAL;
using Code.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Controllers
{
    public class ContactController : Controller
    {
        private readonly AppDbContext _context;
        public ContactController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(Contact contact)
        {
            if (!ModelState.IsValid)
            {
                return View(contact);
            }
            contact.SendedTime = DateTime.Now.ToString("yyyy/MM/dd");
            await _context.Contacts.AddAsync(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Contact));
        }
    }
}
