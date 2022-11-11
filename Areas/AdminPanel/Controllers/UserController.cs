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
    public class UserController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public UserController(AppDbContext context,UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            List<AppUser> appUsers = await _userManager.Users.ToListAsync();
            List<UserVM> userVMs = new List<UserVM>();

            foreach (AppUser appUser in appUsers)
            {
                UserVM userVM = new UserVM
                {
                    Id=appUser.Id,
                    UserName=appUser.UserName,
                    FullName=appUser.FullName,
                    Email=appUser.Email,
                    IsDeleted=appUser.IsDeleted,
                    Role=(await _userManager.GetRolesAsync(appUser))[0]
                };
                userVMs.Add(userVM);
            }
            return View(userVMs);
        }


        public async Task<IActionResult> ChangeStatus(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(string id, bool Status)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.IsDeleted = Status;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ChangeRole(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            UserVM userVM = new UserVM
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                UserName = user.UserName,
                IsDeleted = user.IsDeleted,
                Role = (await _userManager.GetRolesAsync(user))[0],
                Roles = new List<string> { "Admin", "Member" },
            };
            return View(userVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeRole(string id, string Role)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            string oldRole = (await _userManager.GetRolesAsync(user))[0];
            await _userManager.RemoveFromRoleAsync(user, oldRole);
            await _userManager.AddToRoleAsync(user, Role);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> ChangePassword(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(string id, string NewPassword)
        {
            if (id == null)
            {
                return NotFound();
            }
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            await _userManager.ResetPasswordAsync(user, token, NewPassword);
            return RedirectToAction(nameof(Index));
        }
    }
}
