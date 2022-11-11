using Code.Areas.AdminPanel.ViewModels;
using Code.DAL;
using Code.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;
        public DashboardController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            DashboardVM model = new DashboardVM
            {
                Contacts = await _context.Contacts.ToListAsync()
            };

            return View(model);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            Contact contact = await _context.Contacts.FindAsync(id);
            if (contact==null)
            {
                return NotFound();
            }
            return View(contact);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteContact(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            Contact contact = await _context.Contacts.FindAsync(id);
            if (contact==null)
            {
                return NotFound();
            }
            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            MimeMessage emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("HardJob", "elvinnusretzade751@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465, true);
                await client.AuthenticateAsync("elvinnusretzade751@gmail.com", "Nusretzade/669");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }

        public IActionResult SendMessage()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendMessage(ContactVM contact , int? id)
        {
            if (!ModelState.IsValid)
            {
                return View(contact);
            }
            Contact dbcontact = await _context.Contacts.FindAsync(id);
            if (dbcontact == null)
            {
                return View(nameof(Index));
            }
            string MainEmail = dbcontact.Email;
            SendEmailAsync(MainEmail, contact.Subject, contact.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
