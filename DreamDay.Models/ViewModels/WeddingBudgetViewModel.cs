using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.ViewModels
{
    public class WeddingBudgetViewModel
    {
        public int WeddingId { get; set; } // To identify which wedding this budget belongs to

        [Required(ErrorMessage = "Total allocated budget is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Budget must be a positive value.")]
        public decimal TotalAllocatedBudget { get; set; }

        public List<BudgetItemViewModel> Items { get; set; } = new List<BudgetItemViewModel>();

        // For display purposes in the view, not directly part of the form if calculated client-side
        public string? WeddingTitle { get; set; }
    }
}
