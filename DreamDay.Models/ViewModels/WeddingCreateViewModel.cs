using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.ViewModels
{
    public class WeddingCreateViewModel
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public DateTime WeddingDate { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        [StringLength(200)]
        public string Location { get; set; }

        [StringLength(50)]
        public string Theme { get; set; }

        [Range(1, 1000)]
        public int GuestCount { get; set; }

        [Range(0, 1000000)]
        public decimal Budget { get; set; }
    }
}
