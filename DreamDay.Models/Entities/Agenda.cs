using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.Entities
{
    public class Agenda
    {
        public int Id { get; set; }
        public string Title { get; set; }              // e.g., "Ceremony", "Reception", "Vendor Meeting"
        public string Description { get; set; }
        public DateTime StartTime { get; set; }        // When the agenda item starts
        public DateTime EndTime { get; set; }          // When the agenda item ends
        public string Location { get; set; }
        public int WeddingId { get; set; }             // Foreign key to Wedding
        public Wedding Wedding { get; set; }           // Navigation property
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
