using HeyHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyHotel.ViewModels
{
    public class CreateOrderViewModel
    {
        public Room Room { get; set; }
        public int RoomId { get; set; }
        public decimal PersonalDiscount { get; set; }
        public string UserId { get; set; }
        public int NumOfNight { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
    }
}
