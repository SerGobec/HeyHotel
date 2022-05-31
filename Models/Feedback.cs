using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyHotel.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string FeedBack { get; set; }
        public byte Score { get; set; }
        public Order Order { get; set; }
    }
}
