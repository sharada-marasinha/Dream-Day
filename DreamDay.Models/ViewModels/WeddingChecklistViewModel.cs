using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.ViewModels
{
    public class WeddingChecklistViewModel
    {
        public int WeddingId { get; set; }
        public string? WeddingTitle { get; set; }
        public List<WeddingTaskViewModel> Tasks { get; set; } = new List<WeddingTaskViewModel>();
        public WeddingTaskViewModel NewTask { get; set; }
    }
}
