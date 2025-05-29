using DreamDay.DAL.Context;
using DreamDay.DAL.Repositories.Interfaces;
using DreamDay.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AgendaItemRepository : IAgendaItemRepository
{
    private readonly DreamDayDbContext _context;
    public AgendaItemRepository(DreamDayDbContext context) { _context = context; }

    public async Task<IEnumerable<AgendaItem>> GetAllAsync() => await _context.AgendaItems.ToListAsync();
    public async Task<AgendaItem> GetByIdAsync(int id) => await _context.AgendaItems.FindAsync(id);
    public async Task AddAsync(AgendaItem item) { _context.AgendaItems.Add(item); await _context.SaveChangesAsync(); }
    public async Task UpdateAsync(AgendaItem item) { _context.AgendaItems.Update(item); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(int id)
    {
        var item = await _context.AgendaItems.FindAsync(id);
        if (item != null) { _context.AgendaItems.Remove(item); await _context.SaveChangesAsync(); }
    }
}
