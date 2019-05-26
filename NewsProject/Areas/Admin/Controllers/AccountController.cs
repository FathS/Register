using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsProject.Models.Data;
using NewsProject.Models.View;

namespace NewsProject.Areas.Admin.Controllers
{
    [RequireHttps]
    [Authorize]
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        AppDbContext _db;
        LoginModel _lm;
        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, AppDbContext db, LoginModel lm)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
            _lm = lm;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel lm)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(lm.UserName);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, lm.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        ViewData["Error"] = "Kullanıcı Bulunamadı";
                    }
                }
                //ModelState.AddModelError("", "Kullanıcı Bulunamadı");
                return View(lm);

            }
            return View(lm);

        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser();
                user.UserName = model.UserName;
                user.Email = model.Email;

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //_userManager.AddToRoleAsync(user.Id, "Admin");
                    await _db.SaveChangesAsync();
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("RegisterUser", "Kullanıcı Ekleme İşleminde Hata");
                }
                //return View(model);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public IActionResult ChancePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChancePassword(ChancePwdModel model)
        {
            var UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(UserId);
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                ViewData["Success"] = "Şifre Değiştirildi";
                await _db.SaveChangesAsync();
                return View(model);
            }
            return View(model);
        }
        public IActionResult SendPasswordResetLink(string username)
        {
            IdentityUser user = _userManager.FindByNameAsync(username).Result;
            if (user == null || !(_userManager.IsEmailConfirmedAsync(user).Result))
            {
                ViewBag.Message = "Error Resetting your paswoord";
                return View("Error");
            }
            var token = _userManager.GeneratePasswordResetTokenAsync(user).Result;
            var resetLink = Url.Action("ResetPassword", "Account", new { token = token },
                protocol: HttpContext.Request.Scheme);
            ViewBag.Message = "Password reset Link has been sent to your email adress";
            return RedirectToAction("Login", "Account");
        }
    }
}