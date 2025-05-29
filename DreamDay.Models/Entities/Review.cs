using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.Entities
{
    public class Review
    {
        public int Id { get; set; }
        public int Rating { get; set; } // 1-5
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public int VendorId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
