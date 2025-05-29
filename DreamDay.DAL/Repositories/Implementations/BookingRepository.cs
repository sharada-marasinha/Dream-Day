using DreamDay.DAL.Context;
using DreamDay.DAL.Repositories.Interfaces;
using DreamDay.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BookingRepository : IBookingRepository
{
    private readonly DreamDayDbContext _context;
    public BookingRepository(DreamDayDbContext context) { _context = context; }

    public async Task<IEnumerable<Booking>> GetAllAsync() => await _context.Bookings.ToListAsync();
    public async Task<Booking> GetByIdAsync(int id) => await _context.Bookings.FindAsync(id);
    public async Task AddAsync(Booking booking) { _context.Bookings.Add(booking); await _context.SaveChangesAsync(); }
    public async Task UpdateAsync(Booking booking) { _context.Bookings.Update(booking); await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null) { _context.Bookings.Remove(booking); await _context.SaveChangesAsync(); }
    }
}
