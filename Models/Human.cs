using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeyHotel.Models
{
    public class Human
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string SName { get; set; }
        public string Phone { get; set; }
        public string Mail { get; set; }
        public string Role { get; set; }

        List<Transaction> Transactions = new List<Transaction>();

        public Human()
        {
            Transactions = new List<Transaction>();
        }
    }
}
