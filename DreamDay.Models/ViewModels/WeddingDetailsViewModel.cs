using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.ViewModels
{
    public class WeddingDetailsViewModel
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
        public List<WeddingTaskViewModel> Tasks { get; set; }
        public int DaysRemaining => (WeddingDate - DateTime.Today).Days;
    }
}
