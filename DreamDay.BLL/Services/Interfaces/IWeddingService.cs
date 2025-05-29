using DreamDay.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DreamDay.BLL.Services.Interfaces
{
    public interface IWeddingService
    {
        Task<IEnumerable<Wedding>> GetAllAsync();
        Task<Wedding> GetByIdAsync(int id);
        Task<Wedding> AddAsync(Wedding wedding); // <<<< CORRECTED: Returns Task<Wedding>
        Task UpdateAsync(Wedding wedding);
        Task DeleteAsync(int id);
    }
}