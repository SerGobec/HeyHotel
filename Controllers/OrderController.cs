using HeyHotel.Models;
using HeyHotel.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyHotel.Controllers
{
    public class OrderController : Controller
    {
        HotelDbContext _dbContext;
        UserManager<User> _userManager;
        public OrderController(HotelDbContext dbContext, UserManager<User> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ChooseHotel()
        {
            return View(_dbContext.Hotels.ToList());
        }

        public IActionResult ChooseRoom(int? id)
        {
            if(_dbContext.Hotels.Where(el => el.Id == id) != null)
            {
                List<Room> rooms = _dbContext.Rooms.Where(el => el.HotelId == id && !el.IsUsing).ToList();
                return View(rooms);
            }
            return NotFound();
        }

        public async Task<IActionResult> CreateOrder(int? id)
        {
            if (_dbContext.Rooms.Where(el => el.Id == id).FirstOrDefault() != null)
            {
                CreateOrderViewModel model = new CreateOrderViewModel();
                Room room = _dbContext.Rooms.Where(el => el.Id == id).Include(el => el.Hotel).FirstOrDefault();
                User user = await _userManager.FindByNameAsync(HttpContext.User.Identity.Name);
                model.UserId = user.Id;
                model.Room = room;
                model.RoomId = room.Id;
                model.PersonalDiscount = PersonalDiscount(user.Id);
                return View(model);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult ConfirmOrder(CreateOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                ConfirmOrderViewModel ConfirmModel = new ConfirmOrderViewModel();
                ConfirmModel.NumOfNight = model.NumOfNight;
                ConfirmModel.Sum = CalcRoomPrice(model.RoomId, model.UserId, model.NumOfNight);
                ConfirmModel.Room = _dbContext.Rooms.Where(el => el.Id == model.RoomId).FirstOrDefault();
                ConfirmModel.Date = model.Date.Add(model.Time);
                ConfirmModel.RoomId = model.RoomId;
                ConfirmModel.UserId = model.UserId;
                return View(ConfirmModel);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(ConfirmOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                Order order = new Order()
                {
                    Sum = model.Sum,
                    Date = model.Date,
                    IsClosed = false,
                    IsPayed = false,
                    RoomId = model.RoomId,
                    NumOfNight = model.NumOfNight,
                    UserId = model.UserId
                };
                Room room = _dbContext.Rooms.Where(el => el.Id == model.RoomId).FirstOrDefault();
                if(room != null && !room.IsUsing)
                {
                    room.IsUsing = true;

                    _dbContext.Add(order);
                    _dbContext.Update(room);

                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
                return Content("Room alred ordered, or ");
            }
            return View(model);
        }

        public decimal PersonalDiscount(string userId)
        {
            int closedOrders = _dbContext.Orders.Where(el => el.UserId == userId && el.IsPayed && el.IsClosed).Count();
            decimal discount = 1;
            if (closedOrders > 0 && closedOrders < 3) discount = 2;
            if (closedOrders > 3 && closedOrders < 6) discount = 5;
            if (closedOrders > 6 && closedOrders < 10) discount = 7;
            if (closedOrders > 10) discount = 10;
            return discount;
        }

        public decimal CalcRoomPrice(int? roomId, string userId, int numOfDays)
        {
            decimal discount = PersonalDiscount(userId);

            Room room = _dbContext.Rooms.Where(el => el.Id == roomId).FirstOrDefault();
            if(room != null)
            {
                return (room.Price + room.Price * (numOfDays - 1) * 0.95M) * discount;
            }
            return -1;
        }

    }
}
