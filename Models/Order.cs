using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyHotel.Models
{
    public class Order
    {   
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime Date { get; set; }
        public int NumOfNight { get; set; }
        public decimal Sum { get; set; } 

        public User User { get; set; }
        public Room Room { get; set; }
    }
}
