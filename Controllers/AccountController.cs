using Code.Models;
using Code.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager ,RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = await _userManager.FindByEmailAsync(loginVM.Email);
            if (user==null)
            {
                ModelState.AddModelError("","Email ya da Password yalnisdir");
                return View(loginVM);
            }
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager
                .PasswordSignInAsync(user,loginVM.Password,true,true);
            if (result.IsLockedOut)
            {
                ModelState.AddModelError("", "Email Bloklanilib");
                return View(loginVM);
            }
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Email ya da Password yalnisdir");
                return View(loginVM);
            }

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user = new AppUser
            {
                FullName = registerVM.FullName,
                Email = registerVM.Email,
                UserName = registerVM.UserName
            };
            IdentityResult identityResult = await _userManager.CreateAsync(user, registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach (IdentityError error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(registerVM);
            }
            await _userManager.AddToRoleAsync(user, "Member");
            await _signInManager.SignInAsync(user, false);
            return RedirectToAction("Index","Home");
        }
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

        public async Task CreateRole()
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin" });
            }
            if (!await _roleManager.RoleExistsAsync("Member"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Member" });
            }
        }

        //for sending email

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
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Email is incorrect");
                return View(forgotPasswordVM);
            }

            AppUser user = await _userManager.FindByEmailAsync(forgotPasswordVM.Email);

            if (user == null)
            {
                ModelState.AddModelError("Email", "Please try again");
                return View(forgotPasswordVM);
            }

            Random random = new Random();
            int Rnumber = random.Next(1000000);
            string CodeToSend = Rnumber.ToString("D6");

            await SendEmailAsync(forgotPasswordVM.Email, "Password Change", CodeToSend);

            TempData["Code"] = CodeToSend;
            TempData["Email"] = forgotPasswordVM.Email;
            return RedirectToAction(nameof(CheckCode));
        }
        public IActionResult CheckCode()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckCode(string Code)
        {
            string code = TempData["Code"] as string;
            if (Code == code)
            {
                return RedirectToAction(nameof(NewPassword));
            }
            else
            {
                ModelState.AddModelError("", "Code is incorrect");
                return View();
            }
        }
        public IActionResult NewPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewPassword(NewPasswordVM newPasswordVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Check the fields");
                return View(newPasswordVM);
            }

            string email = TempData["Email"] as string;
            AppUser user = await _userManager.FindByEmailAsync(email);

            if (user == null) return NotFound();

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            IdentityResult result = await _userManager.ResetPasswordAsync(user, token, newPasswordVM.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(newPasswordVM);
            }

            await _signInManager.SignInAsync(user, true);
            return RedirectToAction("Index", "Home");
        }
    }
}
