using DreamDay.DAL.Context;
using DreamDay.DAL.Repositories.Interfaces;
using DreamDay.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GuestCategoryRepository : IGuestCategoryRepository
{
    private readonly DreamDayDbContext _context;
    public GuestCategoryRepository(DreamDayDbContext context) { _context = context; }

    public async Task<IEnumerable<GuestCategory>> GetAllAsync() => await _context.GuestCategories.ToListAsync();
    public async Task<GuestCategory> GetByIdAsync(int id) => await _context.GuestCategories.FindAsync(id);
    public async Task AddAsync(GuestCategory category) { _context.GuestCategories.Add(category); await _context.SaveChangesAsync(); }
    public async Task UpdateAsync(GuestCategory category) { _context.GuestCategories.Update(category); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(int id)
    {
        var category = await _context.GuestCategories.FindAsync(id);
        if (category != null) { _context.GuestCategories.Remove(category); await _context.SaveChangesAsync(); }
    }
}
