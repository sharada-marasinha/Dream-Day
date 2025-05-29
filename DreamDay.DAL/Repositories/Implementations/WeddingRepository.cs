using DreamDay.DAL.Context; // Your DbContext namespace
using DreamDay.DAL.Repositories.Interfaces;
using DreamDay.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq; // For FirstOrDefaultAsync
using System.Threading.Tasks;

namespace DreamDay.DAL.Repositories.Implementations // Or your actual namespace
{
    public class WeddingRepository : IWeddingRepository
    {
        private readonly DreamDayDbContext _context;

        public WeddingRepository(DreamDayDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Wedding>> GetAllAsync()
        {
            // Basic implementation. For actual use, you might need .Include() for related data
            // or projections to a ViewModel.
            return await _context.Weddings.AsNoTracking().ToListAsync();
        }

        public async Task<Wedding> GetByIdAsync(int id)
        {
            // This will load the Wedding AND its related collections as defined in OnModelCreating (if lazy loading disabled)
            // or explicitly with .Include()
            return await _context.Weddings
                .Include(w => w.GuestCategories)
                .Include(w => w.Agenda)
                .Include(w => w.SelectedServices)
                .Include(w => w.Tasks) // Assuming Tasks should also be loaded
                .AsNoTracking() // Use AsNoTracking for read-only operations if entity is not going to be updated in this context
                .FirstOrDefaultAsync(w => w.Id == id);
            // If you don't need related data, FindAsync(id) is simpler but won't load them:
            // return await _context.Weddings.FindAsync(id);
        }

        public async Task<Wedding> AddAsync(Wedding wedding) // <<<< CORRECTED: Returns Task<Wedding>
        {
            _context.Weddings.Add(wedding);
            await _context.SaveChangesAsync();
            return wedding; // <<<< CORRECTED: Returns the wedding entity (now with its ID from DB)
        }

        public async Task UpdateAsync(Wedding wedding)
        {
            // For a robust update, especially with child collections, you might need more complex logic.
            // This basic version marks the entire entity as modified.
            _context.Weddings.Update(wedding);
            // Or, if tracking is enabled and wedding was fetched:
            // _context.Entry(wedding).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            // Consider returning Task<Wedding> or bool if you want to confirm success or get the updated entity.
        }

        public async Task DeleteAsync(int id)
        {
            var wedding = await _context.Weddings.FindAsync(id);
            if (wedding != null)
            {
                _context.Weddings.Remove(wedding);
                await _context.SaveChangesAsync();
            }
            // Consider throwing an exception if wedding is null or returning a bool
        }
    }
}