using HeyHotel.Models;
using HeyHotel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyHotel.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            var users = _userManager.Users.ToList();
            return View( users );
        }

        public async Task<IActionResult> UserDetail(string? id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }
            else return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> ChangeRoles(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if(user != null)
            {
                ChangeRolesViewModel model = new ChangeRolesViewModel();
                model.userId = user.Id;
                model.UserName = user.UserName;
                model.AllRoles = _roleManager.Roles.ToList();
                model.UserRoles = await _userManager.GetRolesAsync(user);
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRoles(string UserId, List<string> chosenRoles)
        {
            User user = await _userManager.FindByIdAsync(UserId);
            if (user != null)
            {
                /* var allRoles = _roleManager.Roles.ToList();
                var userRoles = await _userManager.GetRolesAsync(user);
                var addedRoles = chosenRoles.Except(userRoles);
                var removedRoles = allRoles.Except(chosenRoles);
                return View(model);*/

                var userRoles = await _userManager.GetRolesAsync(user);
                var addedRoles = chosenRoles.Except(userRoles);
                var removedRoles = userRoles.Except(chosenRoles);

                await _userManager.AddToRolesAsync(user, addedRoles);
                await _userManager.RemoveFromRolesAsync(user, removedRoles);

                return RedirectToAction("Index", "Admin");
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if(user != null)
            {
                EditUserViewModel model = new EditUserViewModel
                {
                    Id = user.Id,
                    Name = user.Name,
                    Sname = user.SName,
                    userName = user.UserName,
                    mail = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Year = user.Year
                };
                return View(model);
            }
            return NotFound();
        }
        
        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserViewModel model)
        {
            User user = await _userManager.FindByIdAsync(model.Id);
            if(user != null)
            {
                user.Name = model.Name;
                user.SName = model.Sname;
                user.UserName = model.userName;
                user.Email = model.mail;
                user.PhoneNumber = model.PhoneNumber;
                user.Year = model.Year;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }
            return View(model);
        }  
    }
}
