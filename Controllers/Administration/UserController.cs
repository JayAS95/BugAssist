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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPasswordHasher<ApplicationUser> passwordHasher;

        public UserController(UserManager<ApplicationUser> userManager, IPasswordHasher<ApplicationUser> passwordHasher)
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
                ApplicationUser applicationUser = new ApplicationUser
                {
                    UserName = user.UserName,
                    Email = user.Email,
                };

                IdentityResult result = await userManager.CreateAsync(applicationUser, user.Password);

                if (result.Succeeded)
                    return RedirectToAction("ListUsers", "User");
                else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet]
        public IActionResult ListUsers()
        {
            var users = userManager.Users;
            return View(users);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<IActionResult> UpdateUser(string Id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(Id);
            if (user != null)
                return View(user);            
            else
                return RedirectToAction("ListUsers", "User");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(string Id, string UserName, string Email, string Password)
        {
            ApplicationUser user = await userManager.FindByIdAsync(Id);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(UserName))
                    user.UserName = UserName;
                else
                    ModelState.AddModelError("", "Username cannot be empty");

                if (!string.IsNullOrEmpty(Email))
                    user.Email = Email;
                else
                    ModelState.AddModelError("", "Email cannot be empty");

                if (!string.IsNullOrEmpty(Password))
                    user.PasswordHash = passwordHasher.HashPassword(user, Password);
                else
                    ModelState.AddModelError("", "Password cannot be empty");

                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Password))
                {
                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return RedirectToAction("ListUsers", "User");
                    else
                        Errors(result);
                }
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            ApplicationUser user = await userManager.FindByIdAsync(Id);
            if (user != null)
            {
                IdentityResult result = await userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("ListUsers", "User");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View(user);
        }
    }
}