using DreamDay.BLL.Services.Interfaces;
using DreamDay.DAL.Repositories.Interfaces;
using DreamDay.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public class GuestCategoryService : IGuestCategoryService
{
    private readonly IGuestCategoryRepository _guestCategoryRepository;
    public GuestCategoryService(IGuestCategoryRepository guestCategoryRepository)
    {
        _guestCategoryRepository = guestCategoryRepository;
    }

    public Task<IEnumerable<GuestCategory>> GetAllAsync() => _guestCategoryRepository.GetAllAsync();
    public Task<GuestCategory> GetByIdAsync(int id) => _guestCategoryRepository.GetByIdAsync(id);
    public Task AddAsync(GuestCategory category) => _guestCategoryRepository.AddAsync(category);
    public Task UpdateAsync(GuestCategory category) => _guestCategoryRepository.UpdateAsync(category);
    public Task DeleteAsync(int id) => _guestCategoryRepository.DeleteAsync(id);
}
