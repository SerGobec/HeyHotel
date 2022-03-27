using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyHotel.ViewModels
{
    public class EditRoomViewModel
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public decimal Price { get; set; }
        public int NumberOfRooms { get; set; }
        public int Floor { get; set; }
        public string Description { get; set; }
    }
}
