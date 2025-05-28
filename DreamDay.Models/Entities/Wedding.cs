using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.Entities
{
    public class Wedding
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime WeddingDate { get; set; }
        public DateTime StartTime { get; set; }
        public string Location { get; set; }
        public string Theme { get; set; }
        public int GuestCount { get; set; }
        public decimal Budget { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
   
        public ICollection<WeddingTask> Tasks { get; set; }

    }
}
