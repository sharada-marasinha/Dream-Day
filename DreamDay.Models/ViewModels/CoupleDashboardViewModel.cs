using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.Models.ViewModels
{
    public class CoupleDashboardViewModel
    {
        public int WeddingId { get; set; }
        public string? WeddingTitle { get; set; }

        [DataType(DataType.Date)]
        public DateTime? WeddingDate { get; set; }
        public string? WeddingLocation { get; set; }
        public long DaysUntilWedding { get; set; }
        public string? CoverImagePath { get; set; }

        // Checklist Summary
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks => TotalTasks - CompletedTasks;
        public double ChecklistProgressPercent => TotalTasks > 0 ? ((double)CompletedTasks / TotalTasks) * 100 : 0;

        // Budget Summary
        [DataType(DataType.Currency)]
        public decimal TotalAllocatedBudget { get; set; }
        [DataType(DataType.Currency)]
        public decimal TotalActualSpent { get; set; }
        [DataType(DataType.Currency)]
        public decimal RemainingBudget => TotalAllocatedBudget - TotalActualSpent;
        public bool IsOverBudget => TotalActualSpent > TotalAllocatedBudget;

        // Guest List Summary (Conceptual - can be expanded later)
        public int EstimatedGuestCount { get; set; }
        // public int RsvpConfirmedCount { get; set; } // Example for future
    }
}
