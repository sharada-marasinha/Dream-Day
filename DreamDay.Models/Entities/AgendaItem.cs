using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.Entities
{
    public class AgendaItem
    {
        public int Id { get; set; }
        public string Time { get; set; }
        public string Event { get; set; }
        public string Location { get; set; }
        public string Responsible { get; set; }
        public string Notes { get; set; }
        public int WeddingId { get; set; }
        public Wedding Wedding { get; set; }
    }
}
