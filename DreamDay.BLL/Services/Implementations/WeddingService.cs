using DreamDay.BLL.Services.Interfaces;
using DreamDay.DAL.Repositories.Interfaces;
using DreamDay.Models.Entities;
using System; // For ArgumentException example
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Implementations
{
    public class WeddingService : IWeddingService
    {
        private readonly IWeddingRepository _weddingRepository;
        // Optional: private readonly IEmailService _emailService; for notifications

        public WeddingService(IWeddingRepository weddingRepository /*, IEmailService emailService */)
        {
            _weddingRepository = weddingRepository;
            // _emailService = emailService;
        }

        public Task<IEnumerable<Wedding>> GetAllAsync()
        {
            return _weddingRepository.GetAllAsync();
        }

        public Task<Wedding> GetByIdAsync(int id)
        {
            return _weddingRepository.GetByIdAsync(id);
        }

        public async Task<Wedding> AddAsync(Wedding wedding) // <<<< CORRECTED: Returns Task<Wedding>
        {
            // Example of Business Logic:
            if (wedding.WeddingDate.Date < DateTime.Now.AddDays(7))
            {
                throw new ArgumentException("Wedding date and time must be at least 7 days in the future.");
            }
            if (string.IsNullOrWhiteSpace(wedding.Title))
            {
                throw new ArgumentException("Wedding title cannot be empty.");
            }
            // Add other business rules, validations, or orchestrations here.
            // For example, creating default tasks for the wedding:
            // if (wedding.Tasks == null || !wedding.Tasks.Any()) {
            //    wedding.Tasks = new List<WeddingTask> {
            //        new WeddingTask { Title = "Book Venue", DueDate = wedding.WeddingDate.AddMonths(-6), IsCompleted = false },
            //        new WeddingTask { Title = "Send Invitations", DueDate = wedding.WeddingDate.AddMonths(-3), IsCompleted = false }
            //    };
            // }


            var createdWedding = await _weddingRepository.AddAsync(wedding);

            // Example: Post-creation logic (e.g., send a notification)
            // await _emailService.SendWeddingCreationConfirmationAsync(createdWedding.UserId, createdWedding.Title);

            return createdWedding;
        }

        public Task UpdateAsync(Wedding wedding)
        {
            // Add business logic for updates (e.g., validation, checks for status changes)
            if (wedding.Id <= 0)
            {
                throw new ArgumentException("Invalid Wedding ID for update.");
            }
            return _weddingRepository.UpdateAsync(wedding);
        }

        public Task DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Invalid Wedding ID for deletion.");
            }
            // Add any business logic before deletion (e.g., check if deletion is allowed)
            return _weddingRepository.DeleteAsync(id);
        }
    }
}