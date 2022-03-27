using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyHotel.Models
{
    public class Room
    {
        public int Id { get; set; }
        public int HotelId { get; set; }
        public int RoomNumber { get; set; }
        public decimal Price { get; set;}
        public int NumberOfRooms { get; set; }
        public int Floor { get; set; }
        public bool IsUsing { get; set; }
        public string Description { get; set; }

        public Hotel Hotel { get; set; }

        List<Order> Orders = new List<Order>();

        public Room()
        {
            this.Orders = new List<Order>();
        }
    }
}
