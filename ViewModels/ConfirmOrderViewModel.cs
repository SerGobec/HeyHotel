using HeyHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyHotel.ViewModels
{
    public class ConfirmOrderViewModel
    {
        public Room Room { get; set; }
        public string UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime Date { get; set; }
        public int NumOfNight { get; set; }
        public decimal Sum { get; set; }
    }
}
