using DreamDay.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DreamDay.DAL.Repositories.Interfaces
{
    public interface IAgendaItemRepository
    {
        Task<IEnumerable<AgendaItem>> GetAllAsync();
        Task<AgendaItem> GetByIdAsync(int id);
        Task AddAsync(AgendaItem item);
        Task UpdateAsync(AgendaItem item);
        Task DeleteAsync(int id);
    }
}
