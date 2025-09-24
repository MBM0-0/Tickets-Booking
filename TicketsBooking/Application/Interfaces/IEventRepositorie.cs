using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Interfaces
{
    public interface IEventRepositorie
    {
        public Task<List<Event>> GetAllAsync();
        public Task<Event> GetByIdAsync(int id);
        public Task AddAsync(Event CreateEventRequest);
        public Task DeleteAsync(Event entity);
        public Task<bool> GetDuplicateDataAsync(string name);
        public Task SaveChangesAsync();
    }
}
