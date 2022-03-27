using HeyHotel.Models;
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
        public ManagerController(HotelDbContext dbContext)
        {
            this._dbContext = dbContext;
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

        public IActionResult RoomsList(int? id)
        {
            if(id != null)
            {
                List<Room> rooms = _dbContext.Rooms.Where(room => room.HotelId == id).Include(x => x.Hotel).ToList();
                return View(rooms);
            } return NotFound();
        }
    }
}
