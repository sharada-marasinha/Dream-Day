using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.ViewModels
{
    public class BudgetItemViewModel
    {
        public int? Id { get; set; } // For existing items, to help with updates/deletes on backend

        [MaxLength(100, ErrorMessage = "Category cannot exceed 100 characters.")]
        public string Category { get; set; } = "Uncategorized";

        [Required(ErrorMessage = "Item name is required.")]
        [MaxLength(200, ErrorMessage = "Item name cannot exceed 200 characters.")]
        public string ItemName { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Estimated cost must be a positive value.")]
        public decimal EstimatedCost { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Actual cost must be a positive value.")]
        public decimal ActualCost { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Paid amount must be a positive value.")]
        public decimal PaidAmount { get; set; }

        [MaxLength(150, ErrorMessage = "Vendor name cannot exceed 150 characters.")]
        public string? Vendor { get; set; }

        public DateTime? DueDate { get; set; }

        [MaxLength(50, ErrorMessage = "Status cannot exceed 50 characters.")]
        public string? Status { get; set; }

        [MaxLength(500, ErrorMessage = "Notes cannot exceed 500 characters.")]
        public string? Notes { get; set; }
    }
}
