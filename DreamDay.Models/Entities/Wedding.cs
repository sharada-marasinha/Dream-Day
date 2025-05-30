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

        // New properties
        public string? CoverImagePath { get; set; } // Store uploaded image path

        // Navigation properties for new data
        public ICollection<GuestCategory> GuestCategories { get; set; } // New entity
        public bool EnableRSVP { get; set; }
        public ICollection<WeddingService> SelectedServices { get; set; } // New entity for services
        public ICollection<AgendaItem> Agenda { get; set; } // New entity for agenda items

        // Existing navigation
        public ICollection<WeddingTask> Tasks { get; set; }
     
    }
}
