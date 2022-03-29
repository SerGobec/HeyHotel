using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyHotel.Models
{
    public class Order
    {   
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime Date { get; set; }
        public int NumOfNight { get; set; }
        public decimal Sum { get; set; }
        public bool IsPayed { get; set; }
        public bool IsClosed { get; set; }
        public Room Room { get; set; }
    }
}
