using DreamDay.DAL.Context;
using DreamDay.DAL.Repositories.Interfaces;
using DreamDay.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class WeddingServiceRepository : IWeddingServiceRepository
{
    private readonly DreamDayDbContext _context;
    public WeddingServiceRepository(DreamDayDbContext context) { _context = context; }

    public async Task<IEnumerable<WeddingService>> GetAllAsync() => await _context.WeddingServices.ToListAsync();
    public async Task<WeddingService> GetByIdAsync(int id) => await _context.WeddingServices.FindAsync(id);
    public async Task AddAsync(WeddingService service) { _context.WeddingServices.Add(service); await _context.SaveChangesAsync(); }
    public async Task UpdateAsync(WeddingService service) { _context.WeddingServices.Update(service); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(int id)
    {
        var service = await _context.WeddingServices.FindAsync(id);
        if (service != null) { _context.WeddingServices.Remove(service); await _context.SaveChangesAsync(); }
    }
}
