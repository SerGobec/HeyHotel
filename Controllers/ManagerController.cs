﻿using HeyHotel.Models;
using HeyHotel.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyHotel.Controllers
{
    [Authorize(Roles = "manager,admin")]
    public class ManagerController : Controller
    {
        HotelDbContext _dbContext;
        UsersDbContext _usersDbContext;
        public ManagerController(HotelDbContext dbContext, UsersDbContext usersDbContext)
        {
            this._dbContext = dbContext;
            this._usersDbContext = usersDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult HotelList()
        {
            List<Hotel> hotels = _dbContext.Hotels.Include(x => x.Rooms).ToList();
            return View(hotels);
        }

        [HttpGet]
        public IActionResult EditHotel(int id)
        {
            Hotel hotel = _dbContext.Hotels.Where(el => el.Id == id).FirstOrDefault();
            if (hotel != null)
            {
                return View(new EditHotelViewModel()
                {
                    Id = hotel.Id,
                    Location = hotel.Location,
                    Mail = hotel.Mail,
                    PhoneNumber = hotel.PhoneNumber
                });
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditHotel(EditHotelViewModel model)
        {
            if (ModelState.IsValid)
            {
                Hotel hotel = _dbContext.Hotels.Where(el => el.Id == model.Id).FirstOrDefault();
                if(hotel != null)
                {
                    hotel.Location = model.Location;
                    hotel.Mail = model.Mail;
                    hotel.PhoneNumber = model.PhoneNumber;
                    _dbContext.Hotels.Update(hotel);
                    await _dbContext.SaveChangesAsync();
                    return RedirectToAction("HotelList", "Manager");
                }
            }
            return View(model);
        }


        public IActionResult RoomsList(int? id)
        {
            if(id != null)
            {
                List<Room> rooms = _dbContext.Rooms.Where(room => room.HotelId == id).Include(x => x.Hotel).ToList();
                return View("RoomsList", rooms);
            } return NotFound();
        }
        [HttpGet]
        public IActionResult EditRoom(int? id)
        {
            if(id != null)
            {
                Room room = _dbContext.Rooms.Where(room => room.Id == id).FirstOrDefault();
                if(room != null)
                {
                    return View(new EditRoomViewModel()
                    {
                        Id = room.Id,
                        Description = room.Description,
                        Floor = room.Floor,
                        NumberOfRooms = room.NumberOfRooms,
                        Price = room.Price,
                        RoomNumber = room.RoomNumber
                    });
                }
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditRoomPost(EditRoomViewModel model)
        {
            if (ModelState.IsValid)
            {
                Room room = _dbContext.Rooms.Where(room => room.Id == model.Id).FirstOrDefault();
                if(room != null)
                {
                    room.NumberOfRooms = model.NumberOfRooms;
                    room.Price = model.Price;
                    room.RoomNumber = model.RoomNumber;
                    room.Floor = model.Floor;
                    room.Description = model.Description;
                    _dbContext.Update(room);
                    await _dbContext.SaveChangesAsync();
                    //return RedirectToAction("HotelList", "Manager");
                    return RoomsList(room.HotelId);
                }
                return NotFound();
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult RoomDetail(int? id)
        {
            Room room = _dbContext.Rooms.Where(room => room.Id == id).Include(room => room.Hotel).FirstOrDefault();
            if(room != null)
            {
                return View(room);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> DeleteRoom(int? id)
        {
            Room room = _dbContext.Rooms.Where(el => el.Id == id).FirstOrDefault();
            if(room != null)
            {
                _dbContext.Rooms.Remove(room);
                await _dbContext.SaveChangesAsync();
                return RoomsList(room.HotelId);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult OrdersList()
        {
            OrdersListViewModel model = new OrdersListViewModel();

            model.Orders = _dbContext.Orders.Include(el => el.Room).Include(el => el.Room.Hotel).ToList();
            model.UsersInfo = new List<ShortUserInfo>();
            foreach(Order order in model.Orders)
            {
                User user = _usersDbContext.Users.Where(el => el.Id == order.UserId).FirstOrDefault();
                if(user != null)
                {
                    model.UsersInfo.Add(new ShortUserInfo()
                    {
                        Id = user.Id,
                        SName = user.SName,
                        Email = user.Email,
                        Name = user.Name
                    });
                }
            }
            return View(model);
        }
    }
}
