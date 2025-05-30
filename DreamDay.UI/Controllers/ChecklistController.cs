using Microsoft.AspNetCore.Mvc;

namespace DreamDay.UI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    // Using your actual ViewModel namespace:
    // using DreamDay.UI.Models.ViewModels;
    using System.Collections.Generic;
    using System.Linq;
    using System; // For DateTime
    using DreamDay.Models.ViewModels;

    // using Microsoft.AspNetCore.Authorization; // Can be removed if not simulating auth
    // using System.Security.Claims; // Can be removed if not simulating auth

    // --- Mock Data Structures (defined here for simplicity) ---
    public class MockChecklistWedding // Simplified wedding for checklist context
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string MockUserId { get; set; } // Simplified user ID for mock auth
    }

    public class MockChecklistTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
        public int WeddingId { get; set; }
    }
    // --- End Mock Data Structures ---


    // [Authorize] // Remove or comment out if not simulating authorization
    public class ChecklistController : Controller
    {
        // --- Static Mock Data Store ---
        private static List<MockChecklistWedding> _mockWeddings = new List<MockChecklistWedding>
    {
        new MockChecklistWedding { Id = 1, Title = "Alice & Bob's Grand Wedding", MockUserId = "user1@example.com" },
        new MockChecklistWedding { Id = 2, Title = "Charlie & Diana's Rustic Charm", MockUserId = "user2@example.com" }
    };

        private static List<MockChecklistTask> _mockTasks = new List<MockChecklistTask>
    {
        // Tasks for Wedding 1
        new MockChecklistTask { Id = 1, WeddingId = 1, Title = "Book Venue", Description = "Finalize contract with The Grand Hall", DueDate = DateTime.Today.AddDays(30), IsCompleted = true },
        new MockChecklistTask { Id = 2, WeddingId = 1, Title = "Send Invitations", Description = "Mail out all 150 invitations", DueDate = DateTime.Today.AddDays(60), IsCompleted = false },
        new MockChecklistTask { Id = 3, WeddingId = 1, Title = "Finalize Guest List", DueDate = DateTime.Today.AddDays(75), IsCompleted = false },

        // Tasks for Wedding 2
        new MockChecklistTask { Id = 4, WeddingId = 2, Title = "Choose Florist", Description = "Get quotes from 3 florists", DueDate = DateTime.Today.AddDays(20), IsCompleted = false },
        new MockChecklistTask { Id = 5, WeddingId = 2, Title = "Book Photographer", DueDate = DateTime.Today.AddDays(45), IsCompleted = true }
    };
        private static int _nextTaskId = 6; // For generating new task IDs
                                            // --- End Static Mock Data Store ---

        // private readonly ILogger<ChecklistController> _logger; // Optional

        public ChecklistController(/* ILogger<ChecklistController> logger */)
        {
            // _logger = logger;
        }

        // GET: Checklist/Index/{weddingId}
        [HttpGet("Checklist/Index/{weddingId:int}")]
        public IActionResult Index(int weddingId)
        {
            if (weddingId <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Wedding ID.";
                return RedirectToAction("Index", "Home"); // Or your main wedding list or an error page
            }

            var wedding = _mockWeddings.FirstOrDefault(w => w.Id == weddingId);
            if (wedding == null)
            {
                TempData["ErrorMessage"] = "Mock wedding not found.";
                return NotFound();
            }

            // --- Simulate Authorization Check (Optional) ---
            // var currentMockUserId = "user1@example.com"; // Simulate a logged-in user
            // if (wedding.MockUserId != currentMockUserId /* && !User.IsInRole("Admin") */) // Simulate role check
            // {
            //     TempData["ErrorMessage"] = "You are not authorized to view this mock checklist.";
            //     return Forbid(); // Or RedirectToAction("AccessDenied", "Account");
            // }
            // --- End Simulate Authorization Check ---

            var tasksForWedding = _mockTasks.Where(t => t.WeddingId == weddingId).ToList();
            var viewModel = new WeddingChecklistViewModel
            {
                WeddingId = weddingId,
                WeddingTitle = wedding.Title,
                Tasks = tasksForWedding.Select(t => new WeddingTaskViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    DueDate = t.DueDate,
                    IsCompleted = t.IsCompleted,
                    WeddingId = t.WeddingId
                }).OrderBy(t => t.DueDate).ThenBy(t => t.IsCompleted).ToList(),
                NewTask = new WeddingTaskViewModel { WeddingId = weddingId, DueDate = DateTime.Today.AddDays(7) }
            };

            ViewData["Title"] = $"Checklist for {wedding.Title}";
            return View(viewModel);
        }

        // POST: Checklist/AddTask
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddTask(WeddingChecklistViewModel model)
        {
            var weddingId = model.NewTask.WeddingId;
            var wedding = _mockWeddings.FirstOrDefault(w => w.Id == weddingId);

            if (wedding == null)
            {
                TempData["ErrorMessage"] = "Associated mock wedding not found when adding task.";
                return RedirectToAction(nameof(Index), new { weddingId = weddingId }); // Redirect back with error
            }
            // Simulate Authorization as in GET if needed

            // Validate only the NewTask part of the model
            if (string.IsNullOrWhiteSpace(model.NewTask.Title)) // Example of manual validation for mock
            {
                ModelState.AddModelError("NewTask.Title", "Task title is required for mock data.");
            }
            // Add more manual validation for NewTask properties if needed

            if (ModelState.IsValid) // This will check attributes on WeddingTaskViewModel for NewTask
            {
                var newTask = new MockChecklistTask
                {
                    Id = _nextTaskId++,
                    Title = model.NewTask.Title,
                    Description = model.NewTask.Description,
                    DueDate = model.NewTask.DueDate,
                    IsCompleted = false,
                    WeddingId = weddingId
                };
                _mockTasks.Add(newTask);
                TempData["SuccessMessage"] = "Mock task added successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to add task. Please check the details.";
                // To show validation errors properly, we need to pass the full model back
                // Re-populate tasks list for the view
                var tasksForWedding = _mockTasks.Where(t => t.WeddingId == weddingId).ToList();
                model.Tasks = tasksForWedding.Select(t => new WeddingTaskViewModel
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    DueDate = t.DueDate,
                    IsCompleted = t.IsCompleted,
                    WeddingId = t.WeddingId
                }).OrderBy(t => t.DueDate).ThenBy(t => t.IsCompleted).ToList();
                model.WeddingTitle = wedding.Title; // Ensure title is set for view
                ViewData["Title"] = $"Checklist for {wedding.Title}";
                return View(nameof(Index), model); // Return to Index view with the model containing errors
            }
            return RedirectToAction(nameof(Index), new { weddingId = weddingId });
        }

        // POST: Checklist/UpdateTaskStatus (For AJAX)
        [HttpPost("Checklist/UpdateTaskStatus")]
        [ValidateAntiForgeryToken] // Keep for good practice, even with mock
        public IActionResult UpdateTaskStatus(int taskId, bool isCompleted)
        {
            var taskToUpdate = _mockTasks.FirstOrDefault(t => t.Id == taskId);
            if (taskToUpdate == null)
            {
                return Json(new { success = false, message = "Mock task not found." });
            }

            // Simulate authorization: check if the task belongs to an "accessible" wedding
            var wedding = _mockWeddings.FirstOrDefault(w => w.Id == taskToUpdate.WeddingId);
            if (wedding == null /* || wedding.MockUserId != "simulatedCurrentUser" && !isSimulatedAdmin */)
            {
                // return Json(new { success = false, message = "Not authorized for this mock task's wedding." });
            }

            taskToUpdate.IsCompleted = isCompleted;
            // In a real scenario, you'd confirm save success from a service/repo
            return Json(new { success = true, message = "Mock task status updated." });
        }

        // POST: Checklist/DeleteTask/{taskId}
        [HttpPost("Checklist/DeleteTask/{taskId:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteTask(int taskId, [FromForm] int weddingId)
        {
            if (taskId <= 0 || weddingId <= 0) return BadRequest("Invalid IDs.");

            var taskToDelete = _mockTasks.FirstOrDefault(t => t.Id == taskId && t.WeddingId == weddingId);
            if (taskToDelete == null)
            {
                TempData["ErrorMessage"] = "Mock task not found or wedding ID mismatch.";
                return RedirectToAction(nameof(Index), new { weddingId = weddingId });
            }

            // Simulate authorization as in GET/Update if needed

            _mockTasks.Remove(taskToDelete);
            TempData["SuccessMessage"] = "Mock task deleted successfully.";

            return RedirectToAction(nameof(Index), new { weddingId = weddingId });
        }
    }
}
