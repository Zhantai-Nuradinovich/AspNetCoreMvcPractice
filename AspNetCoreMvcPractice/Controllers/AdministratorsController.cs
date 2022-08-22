using AspNetCoreMvcPractice.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreMvcPractice.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministratorsController : Controller
    {
        private readonly UserManager<User> _userManager;
        public AdministratorsController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }
    }
}
