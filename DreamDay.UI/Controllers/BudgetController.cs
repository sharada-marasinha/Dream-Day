namespace DreamDay.UI.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    // Using your actual ViewModel namespace:
    // using DreamDay.UI.Models.ViewModels; // Or DreamDay.Models.ViewModels;
    using System.Collections.Generic;
    using System.Linq;
    using System; // For DateTime
    using Microsoft.AspNetCore.Authorization; // Keep for structure, though auth is simplified
    using DreamDay.Models.ViewModels;

    // Mock Entities (simplified, normally these would be your actual DB entities)
    public class MockWedding
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal TotalAllocatedBudget { get; set; }
        public List<MockBudgetItem> BudgetItems { get; set; } = new List<MockBudgetItem>();
        public string UserId { get; set; } // For simulated authorization
    }

    public class MockBudgetItem
    {
        public int Id { get; set; }
        public string Category { get; set; } = "Uncategorized";
        public string ItemName { get; set; }
        public decimal EstimatedCost { get; set; }
        public decimal ActualCost { get; set; }
        public decimal PaidAmount { get; set; }
        public string? Vendor { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Status { get; set; }
        public string? Notes { get; set; }
    }

    // [Authorize] // You can keep this if you want to test the authorization flow generally
    public class BudgetController : Controller
    {
        // --- Mock Data Store ---
        private static List<MockWedding> _mockWeddings = new List<MockWedding>
    {
        new MockWedding
        {
            Id = 1, Title = "Alice & Bob's Grand Wedding", UserId = "user1", TotalAllocatedBudget = 25000.00m,
            BudgetItems = new List<MockBudgetItem>
            {
                new MockBudgetItem { Id = 1, Category = "Venue", ItemName = "Grand Hall Rental", EstimatedCost = 6000, ActualCost = 6200, PaidAmount = 6200, Status = "Paid", Vendor = "Grand Hall Inc." },
                new MockBudgetItem { Id = 2, Category = "Catering", ItemName = "Dinner for 150 guests", EstimatedCost = 10000, ActualCost = 9500, PaidAmount = 5000, Status = "Booked", Vendor = "Delicious Foods Ltd." },
                new MockBudgetItem { Id = 3, Category = "Photography", ItemName = "Full Day Coverage", EstimatedCost = 3000, ActualCost = 0, PaidAmount = 1000, Status = "Planned", Vendor = "Picture Perfect" }
            }
        },
        new MockWedding
        {
            Id = 2, Title = "Charlie & Diana's Rustic Charm", UserId = "user2", TotalAllocatedBudget = 15000.00m,
            BudgetItems = new List<MockBudgetItem>
            {
                new MockBudgetItem { Id = 4, Category = "Venue", ItemName = "Barn Rental", EstimatedCost = 3000, ActualCost = 3000, PaidAmount = 3000, Status = "Paid" },
                new MockBudgetItem { Id = 5, Category = "Decorations", ItemName = "Flowers & Lights", EstimatedCost = 1500, ActualCost = 0, PaidAmount = 0, Status = "Planned" }
            }
        }
    };
        private static int _nextBudgetItemId = 6; // To simulate ID generation for new budget items


        // GET: Budget/Index/{weddingId}
        // Using attribute routing for clarity and to ensure weddingId is part of the path
        [HttpGet("Budget/Index/{weddingId:int}")]
        public IActionResult Index(int weddingId)
        {
            // Simulate fetching the wedding by ID from our mock store
            var wedding = _mockWeddings.FirstOrDefault(w => w.Id == weddingId);

            if (wedding == null)
            {
                TempData["ErrorMessage"] = $"Wedding with ID {weddingId} not found in mock data.";
                return NotFound(); // Or redirect to a list/home page
            }

            // Simulate Authorization (simplified for mock data)
            // string currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier); // This would be used with real auth
            // For mock, let's assume user1 can see wedding 1, user2 sees wedding 2, or admin sees all
            // if (wedding.UserId != currentUserId && !User.IsInRole("Admin")) { TempData["ErrorMessage"] = "Not authorized."; return Forbid(); }


            var viewModel = new WeddingBudgetViewModel
            {
                WeddingId = wedding.Id,
                WeddingTitle = wedding.Title,
                TotalAllocatedBudget = wedding.TotalAllocatedBudget,
                Items = wedding.BudgetItems.Select(bi => new BudgetItemViewModel
                {
                    Id = bi.Id,
                    Category = bi.Category,
                    ItemName = bi.ItemName,
                    EstimatedCost = bi.EstimatedCost,
                    ActualCost = bi.ActualCost,
                    PaidAmount = bi.PaidAmount,
                    Vendor = bi.Vendor,
                    DueDate = bi.DueDate,
                    Status = bi.Status,
                    Notes = bi.Notes
                }).ToList()
            };

            if (!viewModel.Items.Any()) // Ensure UI always has one row to start if empty
            {
                viewModel.Items.Add(new BudgetItemViewModel());
            }

            ViewData["Title"] = $"Budget for {wedding.Title}";
            return View(viewModel);
        }

        // POST: Budget/Index/{weddingId}
        [HttpPost("Budget/Index/{weddingId:int}")]
        [ValidateAntiForgeryToken]
        public IActionResult Index(int weddingId, WeddingBudgetViewModel model)
        {
            var wedding = _mockWeddings.FirstOrDefault(w => w.Id == weddingId);

            if (wedding == null)
            {
                TempData["ErrorMessage"] = "Wedding not found during save operation.";
                return NotFound();
            }

            // Ensure model has the WeddingTitle for redisplay in case of errors
            model.WeddingTitle = wedding.Title;
            ViewData["Title"] = $"Budget for {wedding.Title}";


            if (!ModelState.IsValid)
            {
                // If items list is null after validation failure, re-initialize for the view
                if (model.Items == null) model.Items = new List<BudgetItemViewModel>();
                if (!model.Items.Any()) model.Items.Add(new BudgetItemViewModel());
                return View(model); // Return with validation errors
            }

            // Simulate Authorization (as in GET)
            // ...

            // "Save" to mock data
            wedding.TotalAllocatedBudget = model.TotalAllocatedBudget;
            wedding.BudgetItems.Clear(); // Simple way: clear existing and add all from model

            if (model.Items != null)
            {
                foreach (var itemVM in model.Items.Where(i => !string.IsNullOrWhiteSpace(i.ItemName)))
                {
                    wedding.BudgetItems.Add(new MockBudgetItem
                    {
                        Id = itemVM.Id ?? _nextBudgetItemId++, // Assign new ID if null, or use existing
                        Category = itemVM.Category,
                        ItemName = itemVM.ItemName,
                        EstimatedCost = itemVM.EstimatedCost,
                        ActualCost = itemVM.ActualCost,
                        PaidAmount = itemVM.PaidAmount,
                        Vendor = itemVM.Vendor,
                        DueDate = itemVM.DueDate,
                        Status = itemVM.Status,
                        Notes = itemVM.Notes
                    });
                }
            }

            TempData["SuccessMessage"] = $"Budget for '{wedding.Title}' (mock data) has been updated!";
            return RedirectToAction(nameof(Index), new { weddingId = wedding.Id });
        }
    }
}
