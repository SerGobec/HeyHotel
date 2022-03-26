using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyHotel.ViewModels
{
    public class CreateRoomViewModel
    {
        public List<SelectListItem> hotelsLocations { get; set; }
        public int HotelId { get; set; }
        public decimal Price { get; set; }
        public int NumberOfRooms { get; set; }
        public int Floor { get; set; }

        public CreateRoomViewModel()
        {
            hotelsLocations = new List<SelectListItem>();
        }
    }
}
