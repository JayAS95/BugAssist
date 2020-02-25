using BugAssist.Models.Administration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugAssist.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private IPasswordHasher<IdentityUser> passwordHasher;

        public UserController(UserManager<IdentityUser> userManager, IPasswordHasher<IdentityUser> passwordHasher)
        {
            this.userManager = userManager;
            this.passwordHasher = passwordHasher;
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserModel user)
        {
            if (ModelState.IsValid)
            {
                IdentityUser IdentityUser = new IdentityUser
                {
                    UserName = user.Name,
                };

                IdentityResult result = await userManager.CreateAsync(IdentityUser);

                if (result.Succeeded)
                    return RedirectToAction("ListUsers", "Administration");
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }

    }
}