using DreamDay.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DreamDay.DAL.Repositories.Interfaces
{
    public interface IWeddingRepository
    {
        Task<IEnumerable<Wedding>> GetAllAsync();
        Task<Wedding> GetByIdAsync(int id); // Consider if this needs to include related data
        Task<Wedding> AddAsync(Wedding wedding); // <<<< CORRECTED: Returns Task<Wedding>
        Task UpdateAsync(Wedding wedding);      // Consider if this should return the updated Wedding
        Task DeleteAsync(int id);
    }
}