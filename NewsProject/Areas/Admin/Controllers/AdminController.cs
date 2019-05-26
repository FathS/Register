using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewsProject.Models.Data;
using NewsProject.Models.View;

namespace NewsProject.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    public class AdminController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        AppDbContext _db;
        
        public AdminController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, AppDbContext db)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _db = db;
            
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult ForgotPassword()
        //{
        //    var user = new IdentityUser();
        //    user = _userManager.FindByIdAsync()
        //}
    }
}