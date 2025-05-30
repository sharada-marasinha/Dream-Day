using Microsoft.AspNetCore.Mvc;

namespace DreamDay.UI.Controllers
{
    using DreamDay.BLL.Services.Interfaces; // Your IWeddingService
    using DreamDay.Models.ViewModels;
    // using DreamDay.UI.Models.ViewModels;  // Your ViewModel namespace
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [Authorize] // Only logged-in users can access the dashboard
    public class CoupleDashboardController : Controller
    {
        private readonly IWeddingService _weddingService;
        // private readonly ILogger<DashboardController> _logger;

        public CoupleDashboardController(IWeddingService weddingService /*, ILogger<DashboardController> logger */)
        {
            _weddingService = weddingService;
            // _logger = logger;
        }

        // GET: /Dashboard or /Dashboard/Index
        public async Task<IActionResult> Index(string userId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(currentUserId))
            {
                // This shouldn't happen if [Authorize] is effective
                return Challenge(); // Or RedirectToAction("Login", "Account");
            }

            // Get the user's active wedding.
            // This service method should load Tasks and BudgetItems.
            var wedding = await _weddingService.GetByIdAsync(2);

            if (wedding == null)
            {
                ViewData["Title"] = "My Dashboard";
                TempData["InfoMessage"] = "You don't have an active wedding planned yet. Why not create one?";
                // Optionally, you could pass a flag or a simplified model to the view
                // to show a "Create Wedding" call to action.
                return View("NoWeddingDashboard"); // A simple view for users without a wedding
            }

            var viewModel = new CoupleDashboardViewModel
            {
                WeddingId = wedding.Id,
                WeddingTitle = wedding.Title,
                WeddingDate = wedding.WeddingDate,
                WeddingLocation = wedding.Location,
                CoverImagePath = wedding.CoverImagePath,
                DaysUntilWedding = wedding.WeddingDate > DateTime.Today ? (long)(wedding.WeddingDate.Date - DateTime.Today.Date).TotalDays : 0,

                // Checklist Summary
                TotalTasks = wedding.Tasks?.Count ?? 0,
                CompletedTasks = wedding.Tasks?.Count(t => t.IsCompleted) ?? 0,

                // Budget Summary
                TotalAllocatedBudget = wedding.Budget, // Assuming Wedding.Budget is the total allocated
                //TotalActualSpent = wedding.BudgetItems?.Sum(bi => bi.ActualCost) ?? 0, // Sum of actual costs from items

                // Guest Summary
                EstimatedGuestCount = wedding.GuestCount
            };

            ViewData["Title"] = $"{wedding.Title} - Dashboard";
            return View(viewModel);
        }
    }
}
