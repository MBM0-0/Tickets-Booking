using TicketsBooking.Application.DTOs.Event;

namespace TicketsBooking.Application.Interfaces
{
    public interface IEventService
    {
        public Task<List<EventResponse>> GetAllEventsAsync();
        public Task<EventResponse> GetEventByIdAsync(int id);
        public Task<EventResponse> CreateEventAsync(CreateEventRequest dto);
        public Task DeleteEventAsync(int id);
        public Task<UpdateEventRequest> UpdateEventAsync(UpdateEventRequest dto);
    }
}
