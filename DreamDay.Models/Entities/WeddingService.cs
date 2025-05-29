using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.Entities
{
    public class WeddingService
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public int WeddingId { get; set; }
        public Wedding Wedding { get; set; }
    }
}
