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
            else return StatusCode(404);
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
            return StatusCode(404);
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
            return StatusCode(404);
        }

    }
}
