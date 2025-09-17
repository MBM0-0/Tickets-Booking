using TicketsBooking.Application.DTOs;
using TicketsBooking.Domain.Entities;

namespace TicketsBooking.Application.Interfaces
{
    public interface IEventRepositorie
    {
        public Task<List<Event>> GetAllAsync();
        public Task AddAsync(Event CreateEventRequest);

    }
}
