using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyHotel.Models
{
    public class Transaction
    {   
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime date { get; set; }
        public decimal Sum { get; set; } 

        public Human Human { get; set; }
        public Room Room { get; set; }
    }
}
