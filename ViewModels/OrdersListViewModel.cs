using HeyHotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyHotel.ViewModels
{
    public class OrdersListViewModel
    {
        public List<Order> Orders { get; set; }
        public List<ShortUserInfo> UsersInfo { get; set; }

        public ShortUserInfo FindUserById(string id)
        {
            return UsersInfo.Where(el => el.Id == id).FirstOrDefault();
        }
    }
}
