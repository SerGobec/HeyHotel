using HeyHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyHotel.ViewModels
{
    public class FeedbackUserPair
    {
        public User User{get; set;}
        public Feedback Feedback { get; set; }
    }
}
