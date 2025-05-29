using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.DAL.Repositories.Interfaces
{
    public interface IWeddingServiceRepository
    {
        Task<IEnumerable<WeddingService>> GetAllAsync();
        Task<WeddingService> GetByIdAsync(int id);
        Task AddAsync(WeddingService service);
        Task UpdateAsync(WeddingService service);
        Task DeleteAsync(int id);
    }
}
