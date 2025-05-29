using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Interfaces
{
    public interface IGuestCategoryService
    {
        Task<IEnumerable<GuestCategory>> GetAllAsync();
        Task<GuestCategory> GetByIdAsync(int id);
        Task AddAsync(GuestCategory category);
        Task UpdateAsync(GuestCategory category);
        Task DeleteAsync(int id);
    }
}
