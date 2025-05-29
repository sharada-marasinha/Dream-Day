using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.Entities
{
    public class GuestCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public int WeddingId { get; set; }
        public Wedding Wedding { get; set; }
    }
}
