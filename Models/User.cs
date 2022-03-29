using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace HeyHotel.Models
{
    public class User : IdentityUser
    {
        public string Name { get; set; }
        public string SName { get; set; }
        public int Year { get; set; }

        List<Order> Orders = new List<Order>();

        public User()
        {
            Orders = new List<Order>();
        }
    }
}
