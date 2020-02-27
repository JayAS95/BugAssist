using BugAssist.Models.Administration;
using BugAssist.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BugAssist.Controllers
{
    public class RoleController : Controller
    {

        private readonly RoleManager<IdentityRole> roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleModel role)
        {
            if (ModelState.IsValid)
            {
                IdentityRole IdentityRole = new IdentityRole
                {
                    Name = role.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(IdentityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles", "Role");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }

            return View(role);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpGet]
        public IActionResult ListRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public async Task<IActionResult> UpdateRole(string Id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(Id);
            if (role != null)
                return View(role);
            else
                return RedirectToAction("ListRoles", "Role");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(string Id, string Name)
        {

            IdentityRole role = await roleManager.FindByIdAsync(Id);
            if (role != null)
            {
                if (!string.IsNullOrEmpty(Name))
                    role.Name = Name;
                else
                    ModelState.AddModelError("", "Role cannot be empty");

                if (!string.IsNullOrEmpty(Name))
                {
                    IdentityResult result = await roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                        return RedirectToAction("ListRoles", "Role");
                    else
                        Errors(result);
                }
            }
            else
                ModelState.AddModelError("", "Role Not Found");
            return View(role);
        }

        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(Id);
            if (role != null)
            {
                IdentityResult result = await roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("ListRoles", "Role");
                else
                    Errors(result);
            }
            else
                ModelState.AddModelError("", "Role Not Found");
            return View(role);
        }
    }
}
