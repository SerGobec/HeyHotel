using HeyHotel.Models;
using HeyHotel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private readonly SignInManager<User> _signInManager;
        private readonly HotelDbContext _dbContext;

        public AdminController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, HotelDbContext dbContext)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._signInManager = signInManager;
            this._dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
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
            if (user != null)
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
            if (user != null)
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
            if (user != null)
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


        [HttpGet]
        public async Task<IActionResult> DeleteUser(string id)
        {
            User user = await _userManager.FindByIdAsync(id);
            if(user != null)
            {
                    DeleteViewModel model = new DeleteViewModel()
                    {
                        Id = user.Id,
                        Name = user.Name,
                        SName = user.SName,
                        Email = user.Email,
                        UserName = user.UserName
                    };
                    return View(model);
            }
            return NotFound();
            
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUserPost(DeleteViewModel model)
        {
            User user = await _userManager.FindByIdAsync(model.Id);
            if(user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Users", "Admin");
                }
                else
                {
                    return NotFound();
                }
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult CreateHotel()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateHotel(CreateHotelViewModel model)
        {
            if (ModelState.IsValid)
            {
                Hotel hotel = new Hotel()
                {
                    Location = model.Location,
                    PhoneNumber = model.PhoneNumber,
                    Mail = model.Mail
                };
                _dbContext.Add(hotel);
                int a = await _dbContext.SaveChangesAsync();
                return RedirectToAction("index", "admin");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult CreateRoom()
        {
            CreateRoomViewModel model = new CreateRoomViewModel();
            foreach(var hotel in _dbContext.Hotels)
            {
                model.hotelsLocations.Add(new SelectListItem
                {
                    Text = hotel.Location,
                    Value = hotel.Id + ""
                });
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoom(CreateRoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                Room room = new Room()
                {
                    Floor = model.Floor,
                    HotelId = model.HotelId,
                    IsUsing = false,
                    NumberOfRooms = model.NumberOfRooms,
                    Price = model.Price,
                    RoomNumber = model.RoomNumber
                };
                if(_dbContext.Rooms.Where(el => el.HotelId == room.HotelId 
                                         && el.RoomNumber == room.RoomNumber)
                                         .FirstOrDefault() == null)
                {
                    _dbContext.Add(room);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("index", "admin");
                }
                return Content("Room already exist");
            }
            return View(model);
        }
    }
}
