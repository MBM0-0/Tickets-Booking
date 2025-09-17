using TicketsBooking.Application.DTOs;

namespace TicketsBooking.Application.Interfaces
{
    public interface IEventService
    {
        public Task<List<EventResponse>> GetAllEventsAsync();
        public Task<EventResponse> CreateEventAsync(CreateEventRequest dto);

    }
}
