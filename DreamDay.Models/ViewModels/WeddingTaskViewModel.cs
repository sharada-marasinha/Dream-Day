using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.ViewModels
{
    public class WeddingTaskViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Task title is required.")]
        [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters.")]
        public string Title { get; set; }

        [StringLength(1000, ErrorMessage = "Description cannot be longer than 1000 characters.")]
        [DataType(DataType.MultilineText)]
        public string? Description { get; set; }

        [Display(Name = "Due Date")]
        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; } = DateTime.Today.AddMonths(1); // Default due date

        public bool IsCompleted { get; set; }

        public int WeddingId { get; set; }
    }
}
