using DreamDay.BLL.Services.Interfaces;
using DreamDay.DAL.Repositories.Interfaces;
using DreamDay.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public class BookingService : IBookingService
{
    private readonly IBookingRepository _bookingRepository;
    public BookingService(IBookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }

    public Task<IEnumerable<Booking>> GetAllAsync() => _bookingRepository.GetAllAsync();
    public Task<Booking> GetByIdAsync(int id) => _bookingRepository.GetByIdAsync(id);
    public Task AddAsync(Booking booking) => _bookingRepository.AddAsync(booking);
    public Task UpdateAsync(Booking booking) => _bookingRepository.UpdateAsync(booking);
    public Task DeleteAsync(int id) => _bookingRepository.DeleteAsync(id);
}
