using DreamDay.BLL.Services.Interfaces;
using DreamDay.Models.Entities;
using DreamDay.Models.ViewModels; // MAKE SURE THIS NAMESPACE IS CORRECT
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // For DbUpdateException

// Define your ViewModels here or in a separate file/folder
// Make sure these match what your Index.cshtml uses and what this controller expects.





[Authorize(Roles = "User,Admin")]
public class WeddingController : Controller
{
    private readonly IWeddingService _weddingService;
    private readonly IWebHostEnvironment _webHostEnvironment;
    // private readonly ILogger<WeddingController> _logger; // Optional for logging

    public WeddingController(IWeddingService weddingService,
                             IWebHostEnvironment webHostEnvironment /*, ILogger<WeddingController> logger */)
    {
        _weddingService = weddingService;
        _webHostEnvironment = webHostEnvironment;
        // _logger = logger;
    }



    [HttpGet]
    public IActionResult Index()
    {
        var model = new WeddingViewModel();
        // Ensure collections are initialized for the view to render empty rows correctly
        if (model.GuestCategories == null || !model.GuestCategories.Any())
        {
            model.GuestCategories.Add(new GuestCategoryViewModel());
        }
        if (model.Agenda == null || !model.Agenda.Any())
        {
            model.Agenda.Add(new AgendaItemViewModel());
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(WeddingViewModel model) // Line 231 is inside this method
    {
        if (model.WeddingDate.Date + model.StartTime < DateTime.Now.AddDays(1)) // Example validation
        {
            ModelState.AddModelError("WeddingDate", "Wedding date and time must be in the future.");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            ModelState.AddModelError("", "Unable to identify the current user. Please log in again.");
            // _logger.LogWarning("UserId was null or empty when attempting to create a wedding.");
            return View(model);
        }

        string relativeCoverImagePath = null;
        if (model.CoverImage != null && model.CoverImage.Length > 0)
        {
            try
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "weddings");
                Directory.CreateDirectory(uploadsFolder);
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.CoverImage.FileName);
                string absoluteFilePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(absoluteFilePath, FileMode.Create))
                {
                    await model.CoverImage.CopyToAsync(fileStream);
                }
                relativeCoverImagePath = "/images/weddings/" + uniqueFileName;
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Error uploading cover image.");
                ModelState.AddModelError("CoverImage", "An error occurred while uploading the cover image.");
                return View(model);
            }
        }

        var weddingEntity = new Wedding
        {
            Title = model.WeddingName,
            Description = model.WeddingDescription ?? string.Empty,
            WeddingDate = model.WeddingDate.Date,
            StartTime = model.WeddingDate.Date + model.StartTime,
            Location = model.WeddingLocation,
            Theme = model.WeddingTheme ?? "Not Specified",
            GuestCount = model.EstimatedGuests,
            Budget = model.TotalBudget,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            CoverImagePath = relativeCoverImagePath,
            EnableRSVP = model.EnableRSVP,
            GuestCategories = new List<GuestCategory>(),
            Agenda = new List<AgendaItem>(),
            SelectedServices = new List<WeddingService>(),
            Tasks = new List<WeddingTask>()
        };

        if (model.GuestCategories != null)
        {
            foreach (var categoryVM in model.GuestCategories.Where(gc => !string.IsNullOrWhiteSpace(gc.Name)))
            {
                weddingEntity.GuestCategories.Add(new GuestCategory { Name = categoryVM.Name, Count = categoryVM.Count });
            }
        }

        if (model.Agenda != null)
        {
            foreach (var agendaVM in model.Agenda.Where(a => !string.IsNullOrWhiteSpace(a.Event)))
            {
                weddingEntity.Agenda.Add(new AgendaItem
                {
                    Time = agendaVM.Time, // Ensure this matches AgendaItem.Time type (string)
                    Event = agendaVM.Event,
                    Location = agendaVM.Location,
                    Responsible = agendaVM.Responsible,
                    Notes = agendaVM.Notes
                });
            }
        }

        if (model.Services != null)
        {
            foreach (var serviceName in model.Services.Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                weddingEntity.SelectedServices.Add(new WeddingService { ServiceName = serviceName });
            }
        }

        try
        {
            // THIS IS THE AREA AROUND YOUR LINE 231
            var createdWedding = await _weddingService.AddAsync(weddingEntity); // This line requires AddAsync to return Task<Wedding>

            TempData["SuccessMessage"] = $"Wedding '{createdWedding.Title}' has been created successfully!";
            return RedirectToAction(nameof(Details), new { id = createdWedding.Id });
        }
        catch (DbUpdateException dbEx)
        {
            // _logger.LogError(dbEx, "Database update error. Inner: {InnerEx}", dbEx.InnerException?.Message);
            ModelState.AddModelError("", "A database error occurred: " + (dbEx.InnerException?.Message ?? dbEx.Message));
            return View(model);
        }
        catch (ArgumentException argEx) // Catch business logic validation errors from service
        {
            // _logger.LogWarning(argEx, "Argument exception during wedding creation.");
            ModelState.AddModelError("", argEx.Message);
            return View(model);
        }
        catch (Exception ex)
        {
            // _logger.LogError(ex, "An unexpected error occurred.");
            ModelState.AddModelError("", $"An unexpected error occurred: {ex.Message}");
            return View(model);
        }
    }

    // In WeddingController.cs

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        if (id <= 0)
        {
            // _logger.LogWarning("Details requested with invalid ID: {Id}", id);
            return BadRequest("Invalid wedding ID provided.");
        }

        // GetByIdAsync in your service should call the repository method
        // which includes GuestCategories, Agenda, SelectedServices, and Tasks.
        var wedding = await _weddingService.GetByIdAsync(id);

        if (wedding == null)
        {
            // _logger.LogWarning("Wedding with ID {WeddingId} not found for Details view.", id);
            TempData["ErrorMessage"] = $"Wedding with ID {id} not found.";
            return NotFound(); // Or return RedirectToAction(nameof(WeddingList)) or another appropriate page
        }

        // Optional: Authorization check to ensure the current user can view this wedding
        var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (wedding.UserId != currentUserId && !User.IsInRole("Admin"))
        {
            // _logger.LogWarning("User {CurrentUserId} attempted to access unauthorized wedding details for ID {WeddingId}", currentUserId, id);
            TempData["ErrorMessage"] = "You are not authorized to view these wedding details.";
            return Forbid(); // Or RedirectToAction("AccessDenied", "Account");
        }

        return View(wedding); // Pass the fetched wedding entity to Details.cshtml
    }
}