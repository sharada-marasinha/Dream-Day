using DreamDay.BLL.Services.Interfaces;
using DreamDay.DAL.Repositories.Interfaces;
using DreamDay.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

public class AgendaItemService : IAgendaItemService
{
    private readonly IAgendaItemRepository _agendaItemRepository;
    public AgendaItemService(IAgendaItemRepository agendaItemRepository)
    {
        _agendaItemRepository = agendaItemRepository;
    }

    public Task<IEnumerable<AgendaItem>> GetAllAsync() => _agendaItemRepository.GetAllAsync();
    public Task<AgendaItem> GetByIdAsync(int id) => _agendaItemRepository.GetByIdAsync(id);
    public Task AddAsync(AgendaItem item) => _agendaItemRepository.AddAsync(item);
    public Task UpdateAsync(AgendaItem item) => _agendaItemRepository.UpdateAsync(item);
    public Task DeleteAsync(int id) => _agendaItemRepository.DeleteAsync(id);
}
