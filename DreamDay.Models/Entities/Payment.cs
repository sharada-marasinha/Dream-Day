using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaidOn { get; set; }
        public string Method { get; set; } // e.g., Credit Card, Bank Transfer
        public int BookingId { get; set; }
        public Booking Booking { get; set; }
    }
}
