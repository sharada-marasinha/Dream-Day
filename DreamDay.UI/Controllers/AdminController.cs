using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class AdminDashboardViewModel
{
    public int TotalUsers { get; set; }
    public int TotalWeddings { get; set; }
    public int TotalBookings { get; set; }
    public int PendingBookings { get; set; }
    public int ConfirmedBookings { get; set; }
}

namespace DreamDay.UI.Controllers
{
    // In Controllers/AdminController.cs
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            // If you created the ViewModel class:
            var viewModel = new AdminDashboardViewModel
            {
                // Populate with dummy data if not fetching from services yet
                TotalUsers = 10,
                TotalWeddings = 5,
                PendingBookings = 2,
                ConfirmedBookings = 3
            };
            return View(viewModel);

            // If you removed @model directive for a quick UI check:
            // return View();
        }

        // Placeholder actions for links to work without error,
        // returning to the dashboard with a message
        public IActionResult ManageUsers()
        {
            TempData["InfoMessage"] = "User Management is under construction.";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ManageWeddings()
        {
            TempData["InfoMessage"] = "Wedding Management is under construction.";
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ManageBookings()
        {
            TempData["InfoMessage"] = "Booking Management is under construction.";
            return RedirectToAction(nameof(Index));
        }
    }
}
